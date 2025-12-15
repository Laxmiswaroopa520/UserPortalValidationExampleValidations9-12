<<<<<<<< HEAD:Services/Interfaces/IUserActivityService.cs
﻿namespace UserPortalValdiationsDBContext.Services.Interfaces
========
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserPortalValdiationsDBContext.Services.Interfaces
>>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc:Application Layer/Services/Interfaces/IUserActivityService.cs
{
    public interface IUserActivityService
    {
        Task<IEnumerable<DateTime>> GetRecentLoginsAsync(int userId, int count);

        Task<Dictionary<string, int>> GetUserCountByDepartmentAsync();
    }
}
