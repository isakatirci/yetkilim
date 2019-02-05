using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Web.Models;

namespace Yetkilim.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IFeedbackService _feedbackService;

        public AccountController(ILogger<AccountController> logger, IUserService userService, IFeedbackService feedbackService)
        {
            _logger = logger;
            _userService = userService;
            _feedbackService = feedbackService;
        }
        public async Task<ViewResult> Profile()
        {
            var model = new ProfileViewModel();

            try
            {
                var userId = CurrentUser.UserId;

                var userRes = await _userService.GetUserByIdAsync(userId);
                if (!userRes.IsSuccess)
                {
                    model.FormMessage = userRes.FormMessage;
                    return View(model);
                }
                var user = userRes.Data;

                model.FullName = user.Name;
                model.IsExternal = user.IsExternal;
                model.CreatedDate = user.CreatedDate;

                var feedbackCountRes = await _feedbackService.GetFeedbackCountByUserIdAsync(userId);
                if (feedbackCountRes.IsSuccess)
                    model.FeedbackCount = feedbackCountRes.Data;


                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profile Edit Error");

                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return View(model);
            }
        }

        public async Task<ViewResult> Edit()
        {
            var model = new ProfileEditViewModel();

            try
            {
                var userId = CurrentUser.UserId;

                var userRes = await _userService.GetUserByIdAsync(userId);
                if (!userRes.IsSuccess)
                {
                    model.FormMessage = userRes.FormMessage;
                    return View(model);
                }

                var user = userRes.Data;

                model.FullName = user.Name;
                model.Email = user.Email;
                model.Phone = user.Phone;
                model.IsExternal = user.IsExternal;

                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profile Edit Error");

                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ViewResult> Edit(ProfileEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var userId = CurrentUser.UserId;

                var userDto = new UserDTO()
                {
                    Name = model.FullName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Password = model.Password
                };

                var userRes = await _userService.UpdateUserAsync(userId, userDto);
                if (!userRes.IsSuccess)
                {
                    model.FormMessage = userRes.FormMessage;
                    return View(model);
                }

                model.IsSuccess = true;
                model.FormMessage = "İşleminiz başarılı bir şekilde gerçekleştirildi.";

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profile Edit Post Error");

                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return View(model);
            }
        }

        public ViewResult ChangePassword()
        {
            var model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ViewResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.NewPassword != model.NewPasswordRe)
            {
                model.FormMessage = "Şifre ve şifre tekrarı aynı olmalıdır.";
                return View(model);
            }

            try
            {
                var userId = CurrentUser.UserId;

                var userRes =
                    await _userService.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

                if (!userRes.IsSuccess)
                {
                    model.FormMessage = userRes.FormMessage;
                    return View(model);
                }

                model.IsSuccess = true;
                model.FormMessage = "İşleminiz başarılı bir şekilde gerçekleştirildi.";

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Change Password Post Error");

                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return View(model);
            }
        }
        
        public async Task<ViewResult> MyFeedbacks()
        {
            var model = new MyFeedbacksViewModel();

            try
            {
                var userId = CurrentUser.UserId;

                var searchModel = new FeedbackSearchModel()
                {
                    PageSize = 40,
                    UserId = userId
                };
                
                var feedbackRes = await _feedbackService.GetAllFeedbackAsync(searchModel);
                if (!feedbackRes.IsSuccess)
                {
                    model.HasError = true;
                    model.FormMessage = feedbackRes.FormMessage;
                    return View(model);
                }

                var feedbacks = feedbackRes.Data;
                if (feedbacks == null || !feedbacks.Any())
                {
                    model.FormMessage = "Hiç değerlendirme yapmadınız. <a href='/home/search'>Mekan arayıp değerlendirme yapmak için tıklayın.</a>";
                    return View(model);
                }

                model.Feedbacks = new List<MyFeedbackViewItem>();

                foreach (var feedback in feedbacks)
                {
                    model.Feedbacks.Add(new MyFeedbackViewItem()
                    {
                        Place = feedback.Place.Name,
                        Description = feedback.Description,
                        CreatedDate = feedback.CreatedDate
                    });
                }

                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MyFeedbacks Error");

                model.HasError = true;
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return View(model);
            }
        }
    }
}