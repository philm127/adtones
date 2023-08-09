using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class DeleteUserRewardCommand.
    /// </summary>
    public class DeleteUserRewardCommand : ICommand
    {
        public int UserRewardId { get; set; }
        public int OperatorId { get; set; }
    }
}