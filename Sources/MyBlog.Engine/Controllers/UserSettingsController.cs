﻿using Microsoft.AspNetCore.Mvc;
using MyBlog.Engine.Services;
using MyLib.Web.Filters;
using System;

namespace MyBlog.Engine.Controllers
{

    [XRobotsTagNoIndexAttribute]
    public class UserSettingsController : Controller
    {
        #region Declarations

        private readonly UserSettingsService _userSettingsService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userSettings"></param>
        public UserSettingsController(UserSettingsService userSettings)
        {
            this._userSettingsService = userSettings;
        }

        #endregion

        #region Cookies 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CookiesConcentClosed()
        {
            // Get current settings
            var settings = _userSettingsService.Get();
            // Set new value
            settings.CookiesConcentClosed = true;
            // Save settings
            _userSettingsService.Set(settings);
            return Json("true");
        }

        #endregion

        #region Manage Accessibility settings

        [HttpGet]
        public ViewResult Accessibility()
        {
            return View(_userSettingsService.Get());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult PostAccessibility()
        {
            return PartialView("_Accessibility", _userSettingsService.Get());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetDyslexicFont(Boolean value)
        {
            // Get current settings
            var settings = _userSettingsService.Get();
            // Set new value
            settings.UseDyslexicFont = value;
            // Save settings
            _userSettingsService.Set(settings);
            return Json("true");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetTextIsJutified(Boolean value)
        {
            // Get current settings
            var settings = _userSettingsService.Get();
            // Set new value
            settings.TextIsJutified = value;
            // Save settings
            _userSettingsService.Set(settings);
            return Json("true");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetLineHeight(Int32 value)
        {
            // Get current settings
            var settings = _userSettingsService.Get();
            // Set new value
            settings.LineHeight = value;
            // Save settings
            _userSettingsService.Set(settings);
            return Json("true");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetZoom(Int32 value)
        {
            // Get current settings
            var settings = _userSettingsService.Get();
            // Set new value
            settings.Zoom = value;
            // Save settings
            _userSettingsService.Set(settings);
            return Json("true");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetTheme(Int32 value)
        {
            // Get current settings
            var settings = _userSettingsService.Get();
            // Set new value
            settings.Theme = value;
            // Save settings
            _userSettingsService.Set(settings);
            return Json("true");
        }

        #endregion

        #region Get style choosed by user

        /// <summary>
        /// Returne the inline style for the layout
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult Style()
        {
            return PartialView("_Style",_userSettingsService.Get());
        }

        #endregion
    }
}