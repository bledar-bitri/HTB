using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.routeplanter;
using HTBUtilities;
using log4net.Config;

namespace HTBRouteInfoLoader
{
    class RouteInfoLoader
    {

        private static readonly RoutePlanerManager RpManager = new RoutePlanerManager(999, true, "99999");
        private readonly int _initialMaxAddresses = 30;
        private readonly int _subsequentAddresses = 10;
        private readonly int _maximumMaxAddresses = 10000;
        private readonly int _maximumRoadsPerThread = 1000;
        private readonly int _sleepTime = 10000;
        private readonly int _sleepTime2 = 100000;
        private readonly int _skipTill = 2940;
        private readonly int _maxAddresses = 30;
        private readonly int _timeToWaitTillRoadLoadingAbort;
        private readonly int _aussendienst = -1;
        private readonly int _initializeSkipAfterThisManyRuns;
        private readonly string _db2Schema;
        private readonly int _runCounter;
        private int _totCounter = 0;
        private Stopwatch _stopwatch = new Stopwatch();
        private RouteInfoLoader()
        {
            XmlConfigurator.Configure(new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Log4Net.config")));

            _initialMaxAddresses = Convert.ToInt32(ConfigurationManager.AppSettings["InitialMaxAddresses"]);
            _subsequentAddresses = Convert.ToInt32(ConfigurationManager.AppSettings["SubsequentAddresses"]);
            _maximumMaxAddresses = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumMaxAddresses"]);
            _maximumRoadsPerThread = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumRoadsPerThread"]);
            _sleepTime = Convert.ToInt32(ConfigurationManager.AppSettings["SleepTime"]);
            _sleepTime2 = Convert.ToInt32(ConfigurationManager.AppSettings["SleepTime2"]);
            _skipTill = Convert.ToInt32(ConfigurationManager.AppSettings["SkipTill"]);
            _aussendienst = Convert.ToInt32(ConfigurationManager.AppSettings["Aussendienst"]);
            _initializeSkipAfterThisManyRuns = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeSkipAfterThisManyRuns"]);
            _timeToWaitTillRoadLoadingAbort = Convert.ToInt32(ConfigurationManager.AppSettings["TimeToWaitTillRoadLoadingAbort"]);
            _db2Schema = ConfigurationManager.AppSettings["DB2Schema"];
            RecordSet.DB2Schema = _db2Schema;

            RoutePlanerManager.TimeToSleepBetweenRefreshes = Convert.ToInt32(ConfigurationManager.AppSettings["RoutePlannerManager_TimeToSleepBetweenRefreshes"]);
            
            _maxAddresses = _initialMaxAddresses;
            _runCounter = 0;
            _stopwatch.Start();
            try
            {
                while (true)
                {
                    LoadRouteInfo();
                    Thread.Sleep(_sleepTime2);
                    _maxAddresses += _subsequentAddresses;
                    if (_maxAddresses == _maximumMaxAddresses)
                        _maxAddresses = _initialMaxAddresses;

                    _runCounter++;
                    if (_runCounter >= _initializeSkipAfterThisManyRuns)
                        _skipTill = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("MAIN ERR: "+e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ReadLine();
                //restart program
//                Process.Start("HTBRouteInfoLoader.exe");
//                Process.GetCurrentProcess().Kill();
            }
        }
        private void LoadRouteInfo()
        {
            RpManager.Clear();
            var adList = GetAussendiestList();
            int adCounter = 0;
            foreach (var ad in adList)
            {
                var aktenList = HTBUtils.GetSqlRecords("SELECT * FROM qryAktenInt WHERE AktIntStatus = 1 AND AktIntSB = " + ad, typeof(qryAktenInt));
//                if (_maxAddresses <= aktenList.Count)
//                {
                    int crrCoutner = 0;
                    var toProcess = new List<qryAktenInt>();
                    foreach (qryAktenInt akt in aktenList)
                    {
                        toProcess.Add(akt);
                        if (crrCoutner >= _maxAddresses)
                        {
                            Console.WriteLine("Processing [AD: {0}] [AD Akten Processed: {1}] [Total: {2}] [AD Count: {3}] [AD Akten: {4}]", ad, adCounter, _totCounter, adList.Count, aktenList.Count);
                            RpManager.Clear();
                            try
                            {
                                if (_totCounter > _skipTill)
                                {
                                    LoadAddresses(toProcess);
                                    RpManager.LoadGeocodeAddresses(false, "HTBRouteInfoLoader.exe");

                                    List<Road> allRoads = RpManager.GetRoads();
                                    int roadCount = 0;
                                    int totRoadCount = 0;
                                    var roadsToProcess = new List<Road>();
                                    foreach(Road r in allRoads)
                                    {
                                        totRoadCount++;
                                        roadsToProcess.Add(r);
                                        if (++roadCount % _maximumRoadsPerThread == 0)
                                        {
                                            Console.WriteLine("Loading Roads [Total: {0}] [Roads: {1} OF {2}]", _totCounter, totRoadCount, allRoads.Count);
                                            RpManager.LoadDistances_NoRoutePlanner(_timeToWaitTillRoadLoadingAbort, _stopwatch, roadsToProcess, false, "HTBRouteInfoLoader.exe");
//                                            Console.WriteLine("Saving Distances");
                                            RpManager.SaveDistances();
//                                            Console.WriteLine("Distances Saved");
                                            roadsToProcess.Clear();
                                            roadCount = 0;
                                            
//                                            Thread.Sleep(_sleepTime); //  just for debug
                                        }
                                    }
//                                    Console.WriteLine("Loading Roads 2");
                                    RpManager.LoadDistances_NoRoutePlanner(_timeToWaitTillRoadLoadingAbort, _stopwatch, roadsToProcess, false, "HTBRouteInfoLoader.exe");
//                                    Console.WriteLine("Saving Distances 2");
                                    RpManager.SaveDistances();
//                                    Console.WriteLine("Distances Saved");
                                    SetConfigValue("SkipTill", _totCounter.ToString());
                                }
                                toProcess.Clear();
                                Console.WriteLine("Done Iteration");
                            }
                            catch
                            {
                                toProcess.Clear();
                            }
                            try
                            {
                                Console.WriteLine("Sleeping: "+_sleepTime);
                                if (_totCounter > _skipTill)
                                    Thread.Sleep(_sleepTime);
                            }
                            catch
                            {
                            }
                            crrCoutner = 0;
                        }
                        adCounter++;
                        crrCoutner++;
                        _totCounter++;
                    }
//                }
//                else
//                {
//                    Console.WriteLine("Skiped AD: {0}", ad);
//                }
                adCounter = 0;
            }
        }

        private void LoadAddresses(List<qryAktenInt> akten)
        {
            foreach (qryAktenInt akt in akten)
            {
                var address = new StringBuilder(HTBUtils.ReplaceStringAfter(HTBUtils.ReplaceStringAfter(akt.GegnerLastStrasse, " top ", ""), "/", ""));
                address.Append(",");
                address.Append(akt.GegnerLastZip);
                address.Append(",");
                address.Append(akt.GegnerLastOrt);
                address.Append(",österreich");

                RpManager.AddAddress(new AddressWithID(akt.AktIntID, address.ToString()));
            }

        }

        private List<string> GetAussendiestList()
        {

            var users = HTBUtils.GetSqlRecords("SELECT * FROM tblUser WHERE UserStatus = 1 " + (_aussendienst > 0 ? "and UserId = " + _aussendienst : ""), typeof(tblUser));
            return (from tblUser user in users select user.UserID.ToString()).ToList();
        }

        private void SetConfigValue(string name, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = config.AppSettings.Settings;

            // update SaveBeforeExit
            settings[name].Value = value;
            
            //save the file
            config.Save(ConfigurationSaveMode.Modified);
            //relaod the section you modified
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }

        static void Main(string[] args)
        {
            new RouteInfoLoader();
        }
    }
}
