using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.DataFactory
{
	public class TransactionWorker : FactoryFloor<Transaction>
    {
        private string HTMLDefaultTemplate = @"<p>Could not find any Receipt Template!</p>";

        public TransactionWorker(PaymentContext context)
        {
            DataContext = context;
        }

        public override Transaction GetById(Guid transactionId)
        {
            return DataContext.Transaction.FirstOrDefault(t => t.TransactionId == transactionId);
        }

        public override int UpdateCreate(Transaction updateRecord)
        {
            if (Exists(updateRecord.TransactionId))
            {
                updateRecord.SyncDate = DateTime.UtcNow;

                DataContext.Transaction.Update(updateRecord);
                return DataContext.SaveChanges();
            }
            else if (updateRecord != null)
            {
                updateRecord.CreatedOn = DateTime.UtcNow;
                DataContext.Transaction.Add(updateRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Delete(Guid guid)
        {
            Transaction existingRecord = GetById(guid);
            if (existingRecord != null)
            {
                existingRecord.Deleted = true;
                existingRecord.DeletedDate = DateTime.UtcNow;

                DataContext.Update(existingRecord);
                return DataContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override bool Exists(Guid guid)
        {
            return DataContext.Transaction.Any(x => x.TransactionId == guid);
        }

        public List<dynamic> RetrieveWithCriteria(string campaignId, string appealId, string packageId, string dateFrom, string dateTo, string cashPaymentCode, string paymentTypeCode, string preferredLanguageCode, string donorSegmentationCode, string addressPresentCode, string businessUnit)
        {
            Guid _campaignId = ConvertStringToGuid(campaignId);
            Guid _appealId = ConvertStringToGuid(appealId);
            Guid _packageId = ConvertStringToGuid(packageId);
            Guid _businessUnit = ConvertStringToGuid(businessUnit);
            DateTime _dateFrom = ConvertStringToDateTime(dateFrom, DateTime.MinValue);
            DateTime _dateTo = ConvertStringToDateTime(dateTo, DateTime.MaxValue);


            // cashPaymentCode == "844060000" => ONLY
            // cashPaymentCode == "844060001" => INCLUDE
            // cashPaymentCode == "844060002" => EXCLUDE
            var cashPaymentTypeList = new int?[] { 844060000, 844060001, 844060002, 844060003, 844060010, 844060011, 844060012, 844060013, 844060014 };

            var transactions = DataContext.Transaction.Where(x => x.TaxReceiptId == null &&
                ((_campaignId == Guid.Empty) || x.OriginatingCampaignId == _campaignId) &&
                ((_appealId == Guid.Empty) || x.AppealId == _appealId) &&
                ((_packageId == Guid.Empty) || x.PackageId == _packageId) &&
                (x.BookDate >= _dateFrom) &&
                (x.BookDate <= _dateTo) &&
                (cashPaymentCode == "844060001" || cashPaymentCode == null ||
                    (cashPaymentCode == "844060000" && cashPaymentTypeList.Contains((int)x.PaymentTypeCode)) ||
                    (cashPaymentCode == "844060002" && (!cashPaymentTypeList.Contains((int)x.PaymentTypeCode) || x.PaymentTypeCode == null)) ||
                    (string.IsNullOrEmpty(cashPaymentCode) && (int)x.PaymentTypeCode == Convert.ToInt32(paymentTypeCode))
                ) &&
                (donorSegmentationCode == "844060001" || donorSegmentationCode == null ||
                    (cashPaymentCode == "844060000" && x.CustomerIdType == 2) ||
                    (cashPaymentCode == "844060002" && x.CustomerIdType == 1)
                ) &&
                ((_businessUnit == Guid.Empty) || x.OwningBusinessUnitId == _businessUnit)
            ).OrderByDescending(x => x.BookDate).ToList<dynamic>();


            // donorSegmentationCode == "844060000" => CONTACT ONLY 
            // donorSegmentationCode == "844060001" => CONTACT & ORGANIZATIONS
            // donorSegmentationCode == "844060002" => ORGANIZATIONS ONLY           

            // addressPresentCode = "844060000" => No filter
            // addressPresentCode = "844060001" => Address1_line1 and Address1_postalcode can not be null

            if (transactions != null)
            {
                if (!string.IsNullOrEmpty(donorSegmentationCode))
                {
                    if (donorSegmentationCode == "844060000") // CONTACTS ONLY
                    {
                        // Filtered by preferredLanguageCode
                        var contacts = (!string.IsNullOrEmpty(preferredLanguageCode)) ? DataContext.Contact.Where(x => x.msnfp_PreferredLanguageCode == Convert.ToInt32(preferredLanguageCode)).ToList() : DataContext.Contact.ToList();
                        // Filtered by addressPresentCode
                        contacts = (!string.IsNullOrEmpty(addressPresentCode) && addressPresentCode == "844060001") ? contacts.Where(x => !string.IsNullOrEmpty(x.Address1_Line1) && !string.IsNullOrEmpty(x.Address1_PostalCode)).ToList() : contacts;
                        // Join found contacts to transactions
                        transactions = transactions.Join(contacts, transaction => transaction.CustomerId, customer => customer.ContactId, (transaction, customer) => new { transaction, customer }).ToList<dynamic>();
                    }
                    else if (donorSegmentationCode == "844060002") // ORGANIZATIONS ONLY
                    {
                        // Filtered by preferredLanguageCode
                        var accounts = (!string.IsNullOrEmpty(preferredLanguageCode)) ? DataContext.Account.Where(x => x.msnfp_PreferredLanguageCode == Convert.ToInt32(preferredLanguageCode)).ToList() : DataContext.Account.ToList();
                        // Filtered by addressPresentCode
                        accounts = (!string.IsNullOrEmpty(addressPresentCode) && addressPresentCode == "844060001") ? accounts.Where(x => !string.IsNullOrEmpty(x.Address1_Line1) && !string.IsNullOrEmpty(x.Address1_PostalCode)).ToList() : accounts;
                        // Join found accounts to transactions
                        transactions = transactions.Join(accounts, transaction => transaction.CustomerId, customer => customer.AccountId, (transaction, customer) => new { transaction, customer }).ToList<dynamic>();
                    }
                    else if (donorSegmentationCode == "844060001") // BOTH CONTACT & ORGANIZATIONS
                    {
                        // Filtered by preferredLanguageCode
                        var contacts = (!string.IsNullOrEmpty(preferredLanguageCode)) ? DataContext.Contact.Where(x => x.msnfp_PreferredLanguageCode == Convert.ToInt32(preferredLanguageCode)).ToList() : DataContext.Contact.ToList();
                        // Filtered by addressPresentCode
                        contacts = (!string.IsNullOrEmpty(addressPresentCode) && addressPresentCode == "844060001") ? contacts.Where(x => !string.IsNullOrEmpty(x.Address1_Line1) && !string.IsNullOrEmpty(x.Address1_PostalCode)).ToList() : contacts;
                        // Join found contacts to transactions
                        var trans1 = transactions.Join(contacts, transaction => transaction.CustomerId, customer => customer.ContactId, (transaction, customer) => new { transaction, customer }).ToList<dynamic>();

                        // Filtered by preferredLanguageCode
                        var accounts = (!string.IsNullOrEmpty(preferredLanguageCode)) ? DataContext.Account.Where(x => x.msnfp_PreferredLanguageCode == Convert.ToInt32(preferredLanguageCode)).ToList() : DataContext.Account.ToList();
                        // Filtered by addressPresentCode
                        accounts = (!string.IsNullOrEmpty(addressPresentCode) && addressPresentCode == "844060001") ? accounts.Where(x => !string.IsNullOrEmpty(x.Address1_Line1) && !string.IsNullOrEmpty(x.Address1_PostalCode)).ToList() : accounts;
                        // Join found accounts to transactions
                        var trans2 = transactions.Join(accounts, transaction => transaction.CustomerId, customer => customer.AccountId, (transaction, customer) => new { transaction, customer }).ToList<dynamic>();

                        // UNION after filtering from contacts and accounts
                        transactions = trans1.Union(trans2).ToList();
                    }
                }
                else // Implement exactly like case donorSegmentationCode == "844060001"
                {
                    // Filtered by preferredLanguageCode
                    var contacts = (!string.IsNullOrEmpty(preferredLanguageCode)) ? DataContext.Contact.Where(x => x.msnfp_PreferredLanguageCode == Convert.ToInt32(preferredLanguageCode)).ToList() : DataContext.Contact.ToList();
                    // Filtered by addressPresentCode
                    contacts = (!string.IsNullOrEmpty(addressPresentCode) && addressPresentCode == "844060001") ? contacts.Where(x => !string.IsNullOrEmpty(x.Address1_Line1) && !string.IsNullOrEmpty(x.Address1_PostalCode)).ToList() : contacts;
                    // Join found contacts to transactions
                    var trans1 = transactions.Join(contacts, transaction => transaction.CustomerId, customer => customer.ContactId, (transaction, customer) => new { transaction, customer }).ToList<dynamic>();

                    // Filtered by preferredLanguageCode
                    var accounts = (!string.IsNullOrEmpty(preferredLanguageCode)) ? DataContext.Account.Where(x => x.msnfp_PreferredLanguageCode == Convert.ToInt32(preferredLanguageCode)).ToList() : DataContext.Account.ToList();
                    // Filtered by addressPresentCode
                    accounts = (!string.IsNullOrEmpty(addressPresentCode) && addressPresentCode == "844060001") ? accounts.Where(x => !string.IsNullOrEmpty(x.Address1_Line1) && !string.IsNullOrEmpty(x.Address1_PostalCode)).ToList() : accounts;
                    // Join found accounts to transactions
                    var trans2 = transactions.Join(accounts, transaction => transaction.CustomerId, customer => customer.AccountId, (transaction, customer) => new { transaction, customer }).ToList<dynamic>();

                    // UNION after filtering from contacts and accounts
                    transactions = trans1.Union(trans2).ToList();
                }
            }

            return transactions.ToList();
        }

        private static Guid ConvertStringToGuid(string value)
        {
            return (!string.IsNullOrEmpty(value) && Guid.TryParse(value.Trim(), out Guid guidValueId))
                ? guidValueId
                : Guid.Empty;
        }

        private static DateTime ConvertStringToDateTime(string value, DateTime defaultDateTime)
        {
            return (!string.IsNullOrEmpty(value) && DateTime.TryParse(value.Trim(), out DateTime dateTime)) ? dateTime : defaultDateTime;
        }

        private static byte[] StreamToBytes(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}
