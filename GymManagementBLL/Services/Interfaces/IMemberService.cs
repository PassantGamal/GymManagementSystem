using GymManagementBLL.ViewModels;
using GymManagementBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();
        bool CreateMember(CreateMemberViewModel member);
        MemberViewModel? GetMemberDetails(int memberId);
        HealthRecordViewModel? GetMemberHealthRecordDetails(int memberId);
        MemberToUpdateViewModel? GetMemberToUpdate(int memberId);
        bool UpdateMemberDetails(int Id, MemberToUpdateViewModel UpdatedMember);
        bool RemoveMember(int MemberId);
    }
}
