// AccountController
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Global.Model;
using Yetkilim.Web.Controllers;
using Yetkilim.Web.Models;

[Authorize]
public class AccountController : BaseController
{
    private readonly ILogger<AccountController> _logger;

    private readonly IUserService _userService;

    private readonly IFeedbackService _feedbackService;

    private readonly IPromotionService _promotionService;

    public AccountController(ILogger<AccountController> logger, IUserService userService, IFeedbackService feedbackService, IPromotionService promotionService)
    {
        _logger = logger;
        _userService = userService;
        _feedbackService = feedbackService;
        _promotionService = promotionService;
    }

    public async Task<ViewResult> Profile()
    {
        ProfileViewModel model = new ProfileViewModel();
        try
        {
            int userId = base.CurrentUser.UserId;
            Result<UserDTO> result = await _userService.GetUserByIdAsync(userId);
            if (!result.IsSuccess)
            {
                model.FormMessage = result.FormMessage;
                return this.View((object)model);
            }
            UserDTO data = result.Data;
            model.FullName = data.Name;
            model.IsExternal = data.IsExternal;
            model.CreatedDate = data.CreatedDate;
            Result<int> result2 = await _feedbackService.GetFeedbackCountByUserIdAsync(userId);
            if (result2.IsSuccess)
            {
                model.FeedbackCount = result2.Data;
            }
            return this.View((object)model);
        }
        catch (Exception ex)
        {
            LoggerExtensions.LogError(_logger, ex, "Profile Edit Error", Array.Empty<object>());
            model.FormMessage = "İşleminiz gerçekleştirilemedi.";
            return this.View((object)model);
        }
    }

    public async Task<ViewResult> Edit()
    {
        ProfileEditViewModel model = new ProfileEditViewModel();
        try
        {
            int userId = base.CurrentUser.UserId;
            Result<UserDTO> result = await _userService.GetUserByIdAsync(userId);
            if (!result.IsSuccess)
            {
                model.FormMessage = result.FormMessage;
                return this.View((object)model);
            }
            UserDTO data = result.Data;
            model.FullName = data.Name;
            model.Email = data.Email;
            model.Phone = data.Phone;
            model.IsExternal = data.IsExternal;
            return this.View((object)model);
        }
        catch (Exception ex)
        {
            LoggerExtensions.LogError(_logger, ex, "Profile Edit Error", Array.Empty<object>());
            model.FormMessage = "İşleminiz gerçekleştirilemedi.";
            return this.View((object)model);
        }
    }

    [HttpPost]
    public async Task<ViewResult> Edit(ProfileEditViewModel model)
    {
        if (this.ModelState.IsValid)
        {
            try
            {
                int userId = base.CurrentUser.UserId;
                UserDTO model2 = new UserDTO
                {
                    Name = model.FullName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Password = model.Password
                };
                Result result = await _userService.UpdateUserAsync(userId, model2);
                if (!result.IsSuccess)
                {
                    model.FormMessage = result.FormMessage;
                    return this.View((object)model);
                }
                model.IsSuccess = true;
                model.FormMessage = "İşleminiz başarılı bir şekilde gerçekleştirildi.";
                return this.View((object)model);
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "Profile Edit Post Error", Array.Empty<object>());
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return this.View((object)model);
            }
        }
        return this.View((object)model);
    }

    public ViewResult ChangePassword()
    {
        ChangePasswordViewModel changePasswordViewModel = new ChangePasswordViewModel();
        return this.View((object)changePasswordViewModel);
    }

    [HttpPost]
    public async Task<ViewResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View((object)model);
        }
        if (!(model.NewPassword != model.NewPasswordRe))
        {
            try
            {
                int userId = base.CurrentUser.UserId;
                Result result = await _userService.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
                if (!result.IsSuccess)
                {
                    model.FormMessage = result.FormMessage;
                    return this.View((object)model);
                }
                model.IsSuccess = true;
                model.FormMessage = "İşleminiz başarılı bir şekilde gerçekleştirildi.";
                return this.View((object)model);
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "Change Password Post Error", Array.Empty<object>());
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return this.View((object)model);
            }
        }
        model.FormMessage = "Şifre ve şifre tekrarı aynı olmalıdır.";
        return this.View((object)model);
    }

    public async Task<ViewResult> MyFeedbacks()
    {
        MyFeedbacksViewModel model = new MyFeedbacksViewModel();
        try
        {
            int userId = base.CurrentUser.UserId;
            CompanyUserSearchModel val = new CompanyUserSearchModel();
            ((SearchModel)val).PageSize = 40;
            val.UserId=(int?)userId;
            CompanyUserSearchModel val2 = val;
            Result<List<FeedbackDetailDTO>> result = await _feedbackService.GetAllFeedbackDetailAsync(val2);
            if (!result.IsSuccess)
            {
                model.HasError = true;
                model.FormMessage = result.FormMessage;
                return this.View((object)model);
            }
            List<FeedbackDetailDTO> data = result.Data;
            if (data == null || !data.Any())
            {
                model.FormMessage = "Hiç değerlendirme yapmadınız. <a href='/home/search'>Mekan arayıp değerlendirme yapmak için tıklayın.</a>";
                return this.View((object)model);
            }
            model.Feedbacks = (from o in data
                               orderby o.CreatedDate descending
                               select o).ToList();
            return this.View((object)model);
        }
        catch (Exception ex)
        {
            LoggerExtensions.LogError(_logger, ex, "MyFeedbacks Error", Array.Empty<object>());
            model.HasError = true;
            model.FormMessage = "İşleminiz gerçekleştirilemedi.";
            return this.View((object)model);
        }
    }

    public async Task<ViewResult> MyPromotions()
    {
        MyPromotionsViewModel model = new MyPromotionsViewModel();
        try
        {
            int userId = base.CurrentUser.UserId;
            CompanyUserSearchModel val = new CompanyUserSearchModel();
            ((SearchModel)val).PageSize = 40;
            val.UserId = (int?)userId;
            CompanyUserSearchModel val2 = val;
            Result<List<PromotionDTO>> result = await _promotionService.GetAllPromotionAsync(val2);
            if (!result.IsSuccess)
            {
                model.HasError = true;
                model.FormMessage = result.FormMessage;
                return this.View((object)model);
            }
            List<PromotionDTO> data = result.Data;
            if (data == null || !data.Any())
            {
                model.FormMessage = "Hiç promosyonunuz yok :( <br/> Üzülmeyin <a href='/home/search'>Mekan arayıp değerlendirip promosyon kazanmak için tıklayın.</a>";
                return this.View((object)model);
            }
            model.Promotions = new List<MyPromotionViewItem>();
            foreach (PromotionDTO item in from o in data
                                          orderby o.Status
                                          select o)
            {
                model.Promotions.Add(new MyPromotionViewItem
                {
                    Place = item.Place.Name,
                    Description = item.Message,
                    DueDate = item.DueDate,
                    Status = item.Status
                });
            }
            return this.View((object)model);
        }
        catch (Exception ex)
        {
            LoggerExtensions.LogError(_logger, ex, "MyPromotions Error", Array.Empty<object>());
            model.HasError = true;
            model.FormMessage = "İşleminiz gerçekleştirilemedi.";
            return this.View((object)model);
        }
    }
}