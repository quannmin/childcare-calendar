using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Domain.ViewModels.ChildrenRecord;

namespace ChildCareCalendar.Domain.ViewModels.CompositeViewModels
{
    public class User_ChildrenCreateViewModel
    {
        public UserCreateViewModel _userCreateViewModel = new();
        public ChildrenRecordCreateViewModel _childCreateViewModel = new();
    }
}
