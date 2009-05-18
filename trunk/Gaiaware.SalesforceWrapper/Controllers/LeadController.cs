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
using System.Collections.Generic;
using Gaiaware.Salesforce;
using Gaiaware.SalesforceWrapper.Utilities;

namespace Gaiaware.SalesforceWrapper.Controllers
{
    public class LeadController : SforceControllerBase
    {

        public string Add(Lead lead)
        {
            // set name to undefined if empty
            if (string.IsNullOrEmpty(lead.Company)) lead.Company = "[undefined]";
            if (string.IsNullOrEmpty(lead.FirstName)) lead.FirstName = "[undefined]";
            if (string.IsNullOrEmpty(lead.LastName)) lead.LastName = "[undefined]";

            return ParseFirstSaveResult(SforceProvider.Instance.Add(lead));
        }

        public string Update(Lead lead)
        {
            return ParseFirstSaveResult(SforceProvider.Instance.Update(lead));
        }

        public string ConvertLeadToContactForAccount(string accountId, Lead lead)
        {
            LeadConvert convert = new LeadConvert
                                      {
                                          accountId = accountId,
                                          leadId = lead.Id,
                                          overwriteLeadSource = false,
                                          doNotCreateOpportunity = true,
                                          sendNotificationEmail = false,
                                          convertedStatus = "Qualified"
                                      };

            LeadConvertResult[] results = SforceProvider.Instance.ConvertLead(convert);

            // one save result with status success returns true
            if (results.Length == 1 && results[0].success)
                return results[0].contactId;
        
            return string.Empty;
        }

        public IEnumerable<Lead> List(string leadSource, DateTime from, DateTime to)
        {
            return
                SforceProvider.Instance.List<Lead>(
                    "SELECT {0} FROM Lead where leadsource='{1}' and CreatedDate > {2} and Createddate < {3}",
                    SforceFields.LeadFields, leadSource, DateTimeIsoFormat.ToIsoFormat(from), DateTimeIsoFormat.ToIsoFormat(to));
        }


        public Lead GetByEmail(string email)
        {
            return SforceProvider.Instance.Get<Lead>("SELECT {0} from Lead where Email='{1}'", SforceFields.LeadFields,
                                                     email);
        }
    }
}