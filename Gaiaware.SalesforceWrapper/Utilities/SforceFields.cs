/*
 * Salesforce API for .NET
 * 
 * Gaiaware also develops Gaia Ajax - the open source Ajax framework for ASP.NET
 * http://gaiaware.net
 * 
 * Copyright (c) 2009 Gaiaware AS.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Text;
using Gaiaware.Salesforce;

namespace Gaiaware.SalesforceWrapper.Utilities
{
    public class SforceFields
    {
		public static string LeadFields
		{
			get { return GetAllFieldsStringForSObject(new string[] { "Lead" }); }
		}

        //TODO: improve the way of doing this
        //public string LeadFields = "id, email, firstname, lastname, leadsource, rating, status, Country, HasOptedOutOfEmail, CreatedDate";
        public const string AccountFields = "id, name, website, phone, billingstreet, billingcity, billingstate, billingpostalcode, billingcountry";
        public const string ContactFields = "id, accountid, name, firstname, lastname, email";
        public const string ContractFields = "id, accountid, ContractNumber, ActivatedDate, companysignedid, customersignedid, createdbyid, createddate, customersigneddate, startdate, enddate, ownerid, status";
        public const string ProductFields = "Name, Description, Id, IsActive, ProductCode";


		private static string GetAllFieldsStringForSObject(string[] sObjects)
		{
			StringBuilder FieldList = new StringBuilder(String.Empty);

			try
			{
				DescribeSObjectResult[] describeSObjectResult = SforceProvider.Instance.SFBinding.describeSObjects(sObjects);

				// Retrieve fields from the results
				Field[] fields = describeSObjectResult[0].fields;

				// Get the name of the object
				String objectName = describeSObjectResult[0].name;

				// Get some flags
				bool isActivateable = describeSObjectResult[0].activateable;
				// Many other values are accessible

				if (fields != null)
				{
					// Add id to the list
					FieldList.Append("Id, ");

					// Iterate through the fields to get properties for each field
					for (int i = 0; i < fields.Length; i++)
					{

						// Boolean	Indicates whether the field is updateable (true) or not (false). If true, then this field value can be set in an update() call.	
						if (fields[i].updateable)
						{ 
							if (i == fields.Length - 1)
							{

								FieldList.Append(fields[i].name);
							}
							else
							{

								FieldList.Append(fields[i].name + ", ");
							}
						}
					}
				}

			}
			catch
			{
				throw (new Exception("Unable to get field names for Salesforce."));
			}

			return FieldList.ToString();
		}

    }
}