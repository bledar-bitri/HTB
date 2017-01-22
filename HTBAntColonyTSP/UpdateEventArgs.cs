using System.Collections.Generic;
using System;

namespace HTBAntColonyTSP
{
	public class UpdateEventArgs : EventArgs
	{

		private readonly int m_CurrentIteration;
		private readonly int m_SuccessfulIterations;
		private readonly int m_Failures;
		private readonly double m_CurrentBestValue;
		private readonly double m_LastValue;
		private readonly IEnumerable<TspCity> m_BestTour;
	    private readonly int m_userId;

		public int CurrentIteration
		{
			get
			{
				return m_CurrentIteration;
			}
		}
		
		public int SuccessfulIterations
		{
			get
			{
				return m_SuccessfulIterations;
			}
		}
		
		public int Failures
		{
			get
			{
				return m_Failures;
			}
		}
		
		public double CurrentBestValue
		{
			get
			{
				return m_CurrentBestValue;
			}
		}
		
		public double LastValue
		{
			get
			{
				return m_LastValue;
			}
		}
		
		public IEnumerable<TspCity> BestTour
		{
			get
			{
				return m_BestTour;
			}
		}
		
	    public int UserID
	    {
            get { return m_userId; }
	    }

		public UpdateEventArgs(int current_iteration, int successful_iterations, int failures, double current_best_value, double last_value, IEnumerable<TspCity> best_tour, int userId = -1)
		{
			m_CurrentIteration = current_iteration;
			m_SuccessfulIterations = successful_iterations;
			m_Failures = failures;
			m_CurrentBestValue = current_best_value;
			m_LastValue = last_value;
			m_BestTour = best_tour;
		    m_userId = userId;
		}
		
		public override string ToString()
		{
			return string.Format("Iteration: {1}{0}w/o change: {2}{0}failed: {3}{0}value: {4:f1}{0}last: {5:f1}", "\t", CurrentIteration, SuccessfulIterations, Failures, CurrentBestValue, LastValue);
		}
		
	}
	
}
