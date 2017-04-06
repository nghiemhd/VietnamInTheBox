using Nop.Core.Domain.Aero87;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Aero87
{
    public interface IGeneralLedgerService
    {
        IList<GeneralLedgerResult> Search(
            DateTime? fromDate,
            DateTime? toDate,
            int? typeId,
            string docNo,
            string acctCode,
            string objCode,
            string objName,
            int pageIndex, int pageSize,
            out int totalRecord
            );
        IList<GeneralLedgerLineResult> GetGeneralLedgerDetail(int tranId);
        
        GeneralLedgerResult GetGeneralLedger(int? docId);

        IList<CF_Acct> AcctAjax(string acc);
        IList<CF_Obj> ObjAjax(string text);

        CF_Acct getAcctById(int id);
        CF_Obj getObjById(int id);

        int EditGeneralLedger(GeneralLedgerResult obj);

        IList<AccountBalanceResult> GetAccountBalance(DateTime fromDate, DateTime toDate, int? objId);

        IList<GeneralLedgerLineResult> GetAcctTransactionDetail(DateTime fromDate, DateTime toDate, int acctId, int? objId, int pageIndex, int pageSize,
            out int totalRecord);
    }
}
