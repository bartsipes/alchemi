using System;
using Alchemi.Core.Owner;

namespace CustomGThreadExample
{
	/// <summary>
	/// This CustomGThread takes a string and converts it to Uppercase.
    /// It serves as an example of how data can be passed to and from a 
    /// GThread, and how and where the actual "processing" code of the
    /// thread exists.
    /// 
    /// Note the [Serializable] attribute, the inherited GThread, and the 
    /// overridden Start() method.
	/// </summary>
	[Serializable]
	public class CustomGThread : Alchemi.Core.Owner.GThread
	{
		private int _index;
		private string _output;
		private string _stringToProcess;

		public CustomGThread( int index, string stringToProcess )
		{
			this._index = index;
			this._stringToProcess = stringToProcess;
		}

		public int Index
		{
			get
			{
				return this._index;
			}
		}

		public string Output
		{
			get
			{
				return this._output;
			}
		}

		public override void Start()
		{
			this._output = this._stringToProcess.ToUpper();
		}
	}
}
