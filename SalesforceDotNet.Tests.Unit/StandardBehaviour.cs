using System;
using NUnit.Framework;

namespace SalesforceDotNet.Tests.Unit
{
	/// <summary>
	///  This is where anything that should be shared globally across all tests
	///  will reside. All tests ultimately inherit from this class.
	/// </summary>
	public class StandardBehaviour
	{
		/// <summary>
		/// This attribute is used inside a TestFixture to provide a common set of functions 
		/// that are performed just before each test method is called.
		/// </summary>
		[SetUp]
		public virtual void SetUp() 
		{ 
		}
		
		/// <summary>
		/// This attribute is used inside a TestFixture to provide a common set of functions 
		/// that are performed after each test method is run.
		/// </summary>
		[TearDown]
		public virtual void TearDown() 
		{ 
		}
	}
}


