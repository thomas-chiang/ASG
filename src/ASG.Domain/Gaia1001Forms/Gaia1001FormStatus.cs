namespace ASG.Domain.Gaia1001Forms;

public enum Gaia1001FormStatus
{
    /// <summary>
    /// 未完成 (Not Complete)
    /// </summary>
    NC,

    /// <summary>
    /// 已儲存 (Save)
    /// </summary>
    SA,

    /// <summary>
    /// 待簽核 (Waiting Approve)
    /// </summary>
    WA,

    /// <summary>
    /// 簽核中 (Under Approving)
    /// </summary>
    UA,

    /// <summary>
    /// 退回 (簽核人拒簽) (Rejected)
    /// </summary>
    RJ,

    /// <summary>
    /// 簽核完成 (Approved)
    /// </summary>
    AP,

    /// <summary>
    /// 撤回 (Recall)
    /// </summary>
    RC,

    /// <summary>
    /// 已刪除 (Deleted)
    /// </summary>
    DE
}