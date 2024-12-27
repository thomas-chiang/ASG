namespace ASG.Domain.Gaia1001Forms;

public enum AttendanceType
{
    /// <summary>
    /// 未知 (Unknown)
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// 上班打卡 (Clock In)
    /// </summary>
    ClockIn = 1,

    /// <summary>
    /// 下班打卡 (Clock Out)
    /// </summary>
    ClockOut = 2,

    /// <summary>
    /// 外出打卡 (Out for Business)
    /// </summary>
    OutForBusiness = 4,

    /// <summary>
    /// 休息開始打卡 (Break Start)
    /// </summary>
    BreakStart = 5,

    /// <summary>
    /// 休息結束打卡 (Break End)
    /// </summary>
    BreakEnd = 6
}