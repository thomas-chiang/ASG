namespace ASG.Domain.Gaia1001Forms
{
    public enum Gaia1001FormStatus
    {
        /// <summary>
        /// 未完成
        /// </summary>
        NotComplete,

        /// <summary>
        /// 已儲存
        /// </summary>
        Save,

        /// <summary>
        /// 待簽核
        /// </summary>
        WaitingApprove,

        /// <summary>
        /// 簽核中
        /// </summary>
        UnderApproving,

        /// <summary>
        /// 退回 (簽核人拒簽)
        /// </summary>
        Rejected,

        /// <summary>
        /// 簽核完成
        /// </summary>
        Approved,

        /// <summary>
        /// 撤回
        /// </summary>
        Recall,

        /// <summary>
        /// 已刪除
        /// </summary>
        Deleted
    }
}