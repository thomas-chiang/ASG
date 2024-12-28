namespace ASG.Domain.Common;

public enum AttendanceType
{
    /// <summary>
    /// 未知 (Unknown)
    /// </summary>
    Unknown,

    /// <summary>
    /// 上班打卡 (Clock In)
    /// </summary>
    ClockIn,

    /// <summary>
    /// 下班打卡 (Clock Out)
    /// </summary>
    ClockOut,

    /// <summary>
    /// 外出打卡 (Out for Business)
    /// </summary>
    OutForBusiness,

    /// <summary>
    /// 休息開始打卡 (Break Start)
    /// </summary>
    BreakStart,

    /// <summary>
    /// 休息結束打卡 (Break End)
    /// </summary>
    BreakEnd
}