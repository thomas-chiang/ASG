namespace ASG.Domain.ApolleAttendanceHistories;

public enum AttendanceMethod
{
    // 未知
    Unknown,

    // 卡鐘打卡
    TimeClock,

    // APP打卡
    App,

    // 定位打卡
    Location,

    // 申請打卡
    Approval,

    // 忘打卡補登
    Retroactive,

    // 匯入
    ClientIn,

    // 網頁打卡
    Web,

    // 平板打卡
    Tablet,

    // 打卡修正
    Correction
}