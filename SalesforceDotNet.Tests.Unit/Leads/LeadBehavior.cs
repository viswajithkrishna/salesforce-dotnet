using System;
using NUnit.Framework;
using Gaiaware.SalesforceWrapper.Controllers;
using Gaiaware.Salesforce;

namespace SalesforceDotNet.Tests.Unit.Leads
{	
	/// <summary>
	/// Lead Behavior test(s).
	/// </summary>
	[TestFixture]
	public class LeadBehavior : StandardBehaviour
	{
		#region tests
		/// <summary>
		/// Add a new Lead.
		/// </summary>
		[Test]
		public void ShouldCreateCreateLead()
		{
			Lead lead = new Lead();
			LeadController leadController = new LeadController();
			
			String leadId;

			lead.FirstName = "Unit";
			lead.LastName = "Test";
			lead.Company = "Unit Test Company";
			lead.Title = "NUnit";
			lead.Phone = "123-456-7890";
			lead.Email = "unit.test@pixelmedia.com";
			lead.Country = "United States";
			lead.State = "New Hampshire";
			lead.LeadSource = "Download";

			// add the lead
			leadId = leadController.Add(lead);
			Assert.IsNotNullOrEmpty(leadId);
		}

		/// <summary>
		/// Find a Lead by email address, update the Company of the Lead.
		/// </summary>
		[Test]
		public void ShouldUpdateLead()
		{
			Lead lead = new LeadController().GetByEmail("unit.test@pixelmedia.com");
			lead.Company = "Unit Test Incorporated";

			LeadController controller = new LeadController();

			string updateLeadResult = controller.Update(lead);
			Assert.IsNotEmpty(updateLeadResult);
		}

		/// <summary>
		/// Find a Lead by email address.
		/// </summary>
		[Test]
		public void ShouldFindLeadByEmail()
		{
			Lead lead = new LeadController().GetByEmail("unit.test@pixelmedia.com");
			Assert.AreEqual(lead.Company, "Unit Test Incorporated");
		}
		#endregion tests
	}
}
