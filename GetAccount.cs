
using Sumeru.Flex;
using System;
using System.Linq;
using ENTiger.Accounts.Messages.DataPackets;
using ENTiger.Accounts.DomainModels.Paras;
using ENTiger.Accounts.ExtensionMethods;
using ENTiger.ENCollect;
using ENTiger.Accounts.OutputAPIModels;
using System.Reflection;
//Test
namespace ENTiger.Accounts.Plugins.Paras
{
    public class GetAccount : IFlexiFlowModule<GetAccountDataPacket>
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public bool Execute(GetAccountDataPacket packet)
        {
            int intResult;
            string guid = Guid.NewGuid().ToString();
            string startTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            string middleTime = string.Empty;
            string endTime = string.Empty;

            ParasLoanAccount account;
            try
            {
                using (IFlexRepository _repo = InitFlex.factory.FlexRepository)
                {
                    
                    account = _repo.FindAll<ParasLoanAccount>()
                             .ByAccountNo(packet.account)
                             .Select(p => new
                             {
                                 p.CustomId,
                                 p.CUSTOMERNAME,
                                 p.EMIAMT,
                                 p.CURRENT_BUCKET,
                                 p.BUCKET,
                                 p.BOM_POS,
                                 p.PRODUCT,
                                 p.MAILINGLANDMARK,
                                 p.MAILINGADDRESS,
                                 p.CITY,
                                 p.STATE,
                                 p.CURRENT_DPD,
                                 p.Id,
                                 p.MAILINGMOBILE,
                                 p.LatestEmailId,
                                 p.LatestMobileNo,
                                 p.LatestPTPDate

                             }).ToList()
                             .Select(a => new ParasLoanAccount
                             {
                                 CustomId = a.CustomId,
                                 CUSTOMERNAME = a.CUSTOMERNAME,
                                 EMIAMT = a.EMIAMT,
                                 CURRENT_BUCKET = a.CURRENT_BUCKET,
                                 BUCKET = a.BUCKET,
                                 BOM_POS = a.BOM_POS,
                                 PRODUCT = a.PRODUCT,
                                 MAILINGLANDMARK = a.MAILINGLANDMARK,
                                 MAILINGADDRESS = a.MAILINGADDRESS,
                                 CITY = a.CITY,
                                 STATE = a.STATE,
                                 CURRENT_DPD = a.CURRENT_DPD,
                                 Id = a.Id,
                                 MAILINGMOBILE = a.MAILINGMOBILE,
                                 LatestEmailId = a.LatestEmailId,
                                 LatestMobileNo = a.LatestMobileNo,
                                 LatestPTPDate = a.LatestPTPDate

                             })
                             .FirstOrDefault();

                    middleTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                }

                if (account != null)
                {

                    packet.accountDetails = new SearchAccountOutputAPIModel();

                    packet.accountDetails.AccountNo = account.CustomId;
                    packet.accountDetails.CustomerName = account.CUSTOMERNAME;
                    packet.accountDetails.EMIAmount = String.IsNullOrEmpty(account.EMIAMT) ? "0" : account.EMIAMT;
                    packet.accountDetails.CurrentBucket = account.CURRENT_BUCKET;
                    packet.accountDetails.MonthStartingBucket = Int32.TryParse(account.BUCKET.ToString(), out intResult) ? intResult : 0;
                    packet.accountDetails.POS = account.BOM_POS;
                    packet.accountDetails.ProductName = account.PRODUCT;
                    packet.accountDetails.Area = account.MAILINGLANDMARK;
                    packet.accountDetails.Address = account.MAILINGADDRESS;
                    packet.accountDetails.City = account.CITY;
                    packet.accountDetails.State = account.STATE;
                    packet.accountDetails.CurrentDPD = account.CURRENT_DPD;
                    packet.accountDetails.Id = account.Id;
                    packet.accountDetails.EMailId = account.LatestEmailId;
                    packet.accountDetails.MobileNo = account.LatestMobileNo != null ? account.LatestMobileNo : account.MAILINGMOBILE;

                }
            }
            catch(Exception ex)
            {
                packet.AddError("getaccount", ex.StackTrace + ex.InnerException + ex);
            }

            endTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            //logger.Info("AccountSearch FF Start : " + startTime
            //    + System.Environment.NewLine 
            //    + " Middle : " + middleTime
            //    +System.Environment.NewLine
            //    + " End : " + endTime
            //    +System.Environment.NewLine
            //    + " : Accountno " + packet.account);
            return true;
        }
    }
}
