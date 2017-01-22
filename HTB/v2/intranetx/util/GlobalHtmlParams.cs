namespace HTB.v2.intranetx.util
{
    public sealed class GlobalHtmlParams
    {
        public const string USER = "User";
        public const string NAME = "Name";
        public const string USER_NAME = "UserName";
        public const string PASSWORD = "Pwd";


        public const string AUFTRAGGEBER_ID = "AGID";
        public const string GEGNER_ID = "GegnerID";
        public const string GEGNER2_ID = "Gegner2ID";
        public const string CLIENT_ID = "ClientID";
        public const string USER_ID = "UserID";
        public const string DOCUMENT_ID = "DocID";
        public const string DEALER_ID = "DealerID";
        public const string GEGNER_PHONE_ID = "GegnerPhoneID";

        public const string CLIENT_INVOICE_NUMBER = "ClientInvNbr";
        public const string CLIENT_INVOICE_DATE = "ClientInvDte";
        public const string CLIENT_INVOICE_AMOUNT = "ClientInvAmt";
        public const string CLIENT_EXTRA_CHARGES = "ClientExtraAmt";
        public const string CLIENT_REFERENCE_NUMBER = "ClientRefNbr";

        public const string INKASSO_AKT_MAIN_STATUS = "IAMS";
        public const string INKASSO_AKT_CURRENT_STATUS = "IACS";
        public const string INKASSO_AKT_NEXT_ACTION_DATE = "IANAD";
        public const string INKASSO_AKT_STOP_WORKFLOW = "IASW";
        public const string INKASSO_MAIN_SCREEN = "InkMainScreen";
        public const string INKASSO_EDIT_AKT = "InkEdtAkt";
        public const string INKASSO_AKT = "InkAkt";

        public const string INTERVENTION_WORK_AKT = "IntWrkAkt";
        public const string REFRESH_INTERVENTION_WORK_AKT = "RfrIntWrkAkt";
        public const string REFRESH_INKASSO_WORK_AKT = "RfrInkWrkAkt";
        public const string INTERVENTION_AKT = "IntAkt";

        public const string ROAD_NAME = "RoadName";
        public const string INKASSANT_ID = "InkassantId";

        public const string MAHNUNG_NUMBER = "MahNbr";

        public const string BACK = "back";

        public const string MEMO = "memo";
        public const string ID = "ID";
        public const string ADDRESS_ID = "AdrId";
        public const string PHONE_ID = "PhoneId";
        public const string LAWYER_ID = "LawyerId";

        public const string IS_POPUP = "pop";
        public const string RETURN_TO_URL_ON_SUBMIT = "RetSbmt";
        public const string RETURN_TO_URL_ON_CANCEL = "RetCncl";
        public const string EXTRA_PARAMS = "extra";
        public const string YES = "Y";
        public const string SUBMIT_ON_CLOSE = "SubmitOnClose";
        public const string SEARCH_FOR = "SearchFor";
        public const string SEARCH_FOR_NAME = "SearchForName";
        public const string SEARCH_FOR_NUMBER = "SearchForNumber";
        public const string SEARCH_FOR_PHONE = "SearchForPhone";
        public const string NEW_SEARCH = "NewSearch";
        public const string RETURN_GEGNER2 = "SendG2";
        public const string CLOSE_WINDOW = "CloseWindow";


        public const string URL_NEW_AKT = "NewAkt";
        public const string URL_NEW_AKT_INT_AUTO = "NewAktInt";
        public const string URL_NEW_GEGNER = "NewGegner";
        public const string URL_EDIT_GEGNER = "EditGegner";
        public const string URL_BROWSER_GEGNER = "BrowserGegner";
        public const string URL = "URL";

        public const string ACTION_NAME = "ActionName";
        public const string ACTION_MEMO = "ActionMemo";
        public const string ACTION_MELDE = "ActionMelde";
        public const string ACTION_INTERVENTION = "ActionInt";
        public const string ACTION_CALCULATE_INSTALLMENT_BASED_ON_NUMBER_OF_INSTALLMENTS = "ActionCalcOnNumbOfInst";
        public const string ACTION_CALCULATE_INSTALLMENT_BASED_ON_INSTALLMENT_AMOUNT = "ActionCalcInstAmnt";
        public const string ACTION_GET_INSTALLMENT_INFO = "ActionGetInstInfo";
        public const string ACTION_GENERATE_RV_PDF = "ActionGenerateRvPdf";

        public const string ACTION_TYPE_ID = "ActionTypeId";
        public const string AKT_TYPE_ID = "AktTypeId";
        public const string SELECTED_VALUE = "SelVal";

        public const string LATITUDE = "LAT";
        public const string LONGITUDE = "LNG";
        
        public const string INCLUDE_ABGEGEBENE_AKTE = "IAA";

        public const string START_DATE = "StrDte";
        public const string END_DATE = "EndDte";
        public const string NUMBER_OF_INSTALLMENTS = "NOI";
        public const string INSTALLMENT_AMOUNT = "InstAmt";
        public const string INSTALLMENT_PERIOD = "InstPeriod";
        public const string INSTALLMENT_PERIOD_WEEKLY = "w";
        public const string INSTALLMENT_PERIOD_MONTHLY = "m";
        public const string PAID = "Paid";


        public const string INSTALLMENT_RV_NAME = "IRVNAME";
        public const string INSTALLMENT_RV_ADDRESS = "IRVADDR";
        public const string INSTALLMENT_RV_PHONE_COUNTRY = "IRVPHONECOU";
        public const string INSTALLMENT_RV_PHONE_CITY = "IRVPHONECITY";
        public const string INSTALLMENT_RV_PHONE = "IRVPHONE";
        public const string INSTALLMENT_RV_DATE_OF_BIRTH = "IRVDOB";
        public const string INSTALLMENT_RV_SVA = "IRVSVA";
        public const string INSTALLMENT_RV_JOB_DESCRIPTION = "IRVJD";
        public const string INSTALLMENT_RV_EMPLOYER = "IRVEMP";

        public const string INSTALLMENT_RV_NAME_PARTNER = "IRVNAME_P";
        public const string INSTALLMENT_RV_ADDRESS_PARTNER = "IRVADDR_P";
        public const string INSTALLMENT_RV_PHONE_COUNTRY_PARTNER = "IRVPHONECOU_P";
        public const string INSTALLMENT_RV_PHONE_CITY_PARTNER = "IRVPHONECITY_P";
        public const string INSTALLMENT_RV_PHONE_PARTNER = "IRVPHONE_P";
        public const string INSTALLMENT_RV_DATE_OF_BIRTH_PARTNER = "IRVDOB_P";
        public const string INSTALLMENT_RV_SVA_PARTNER = "IRVSVA_P";
        public const string INSTALLMENT_RV_JOB_DESCRIPTION_PARTNER = "IRVJD_P";
        public const string INSTALLMENT_RV_EMPLOYER_PARTNER = "IRVEMP_P";

        public const string XML_DATA = "XMLDATA";
        public const string PROTOCOL_TYPE = "ProtocolType";
        public const string PROTOCOL_TYPE_UBERNAME = "ProtTypeUbername";

        public const string DOWNLOAD_TYPE = "DownloadType";
        public const string DOWNLOAD_PHONE_TYPES = "DPT";
        public const string DOWNLOAD_AKT_TYPES = "DAT";
        public const string DOWNLOAD_ACTION_TYPES = "DACT";
        public const string DOWNLOAD_FIELD_PERSONS = "DAFP";

        public const string MESSAGE_TYPE = "MessageType";
        public const string MESSAGE_ADD_GEGNER_PHONE_NUMBER = "MAGPN";
        public const string MESSAGE_DELETE_GEGNER_PHONE_NUMBER = "MDGPN";
        public const string MESSAGE_SET_GEGNER_EMAIL = "MSGEMAIL";
        public const string MESSAGE_SET_GEGNER_PARTNER_EMAIL = "MSSGPEMAIL";
        
        public const string SOURCE = "Source";
        public const string SHOW_GRID = "ShowGrid";
        public const string TIME_START = "TimStr";
        public const string TIME_END = "TimEnd";
        public const string ZIP_CODE_RANGE = "ZCR";
        public const string REVERSE_ROUTE = "RevRoute";

        public const string BingRouteRecalculate = "BingRouteRecalculate";
        public const string RoutePlanner = "RoutePlanner";

        public const string SessionRoutePlannerManager = "RoutePlannerManager";
        public const string SessionCurrentPosition = "CurrentPosition";
        public const string SessionReplacementAddresses = "ReplacementAddresses";
        
        public const string ResponseOK = "OKK";
        public const string ResponseError = "ERROR:";

       
    }
}