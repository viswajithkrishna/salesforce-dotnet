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

using System.Collections.Generic;
using Gaiaware.Salesforce;
using Gaiaware.SalesforceWrapper.Utilities;

namespace Gaiaware.SalesforceWrapper.Controllers
{
    public class ContactController : SforceControllerBase
    {
       
        public Contact GetByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            return
                SforceProvider.Instance.Get<Contact>(
                    "select {0} from contact where username__c='{1}'", SforceFields.ContactFields, username);
        }

        public Contact Get(string id)
        {
            return
                SforceProvider.Instance.Get<Contact>(
                    "select {0} from contact where id = '{1}'", SforceFields.ContactFields, id);
        }


        public string Update(Contact contact)
        {
            return ParseFirstSaveResult(SforceProvider.Instance.Update(contact));
        }

        public IEnumerable<Contact> GetContactsByAccountId(string accountId)
        {
            return SforceProvider.Instance.List<Contact>("select {0} from contact where accountid='{1}'",
                                                         SforceFields.ContactFields,
                                                         accountId);
        }

        public string Add(Contact newContact)
        {
            return ParseFirstSaveResult(SforceProvider.Instance.Add(newContact));
        }


    }
}