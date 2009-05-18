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
using System;
using Gaiaware.Salesforce;
using Gaiaware.SalesforceWrapper.Utilities;

namespace Gaiaware.SalesforceWrapper.Controllers
{
    public class ContractController : SforceControllerBase
    {
        public string Add(Contract contract)
        {
            return ParseFirstSaveResult(SforceProvider.Instance.Add(contract));
        }

        /// <summary>
        /// Retrieves all contracts from SalesForce based on a given account id
        /// </summary>
        public IEnumerable<Contract> GetContractsByAccountId(string accountId)
        {
            return SforceProvider.Instance.List<Contract>("select {0} from contract where accountid='{1}'",
                                                          SforceFields.ContractFields,
                                                          accountId);
        }

        public Contract Get(string contractId)
        {
            return SforceProvider.Instance.Get<Contract>(
                "select {0} from contract where id = '{1}'",
                SforceFields.ContractFields,
                contractId);
        }

        public bool Activate(string contractId)
        {
            var activateDate = DateTime.Now;

            var contractToActivate = new Contract
                                         {
                                             Id = contractId,
                                             ActivatedDate = activateDate,
                                             ActivatedDateSpecified = true,
                                             Status = "Activated",
                                             OwnerExpirationNotice = "30",
                                             StartDate = activateDate,
                                             StartDateSpecified = true
                                         };

            var result = SforceProvider.Instance.Update(contractToActivate);

            return !String.IsNullOrEmpty(ParseFirstSaveResult(result));
        }
    }
}