namespace ASG.Domain.Common.Enums;

public enum AttendanceType
{
    // 未知 (Unknown)
    Unknown = 0,

    // 上班打卡 (Clock In)
    ClockIn = 1,

    // 下班打卡 (Clock Out)
    ClockOut = 2,

    // 外出打卡 (Out for Business)
    OutForBusiness = 4,

    // 休息開始打卡 (Break Start)
    BreakStart = 5,

    // 休息結束打卡 (Break End)
    BreakEnd = 6
}