﻿using eKnjiznica.AdminUI.Services.API;
using eKnjiznica.Commons.Util;
using eKnjiznica.AdminUI.Services.User;
using eKnjiznica.Commons;
using eKnjiznica.Commons.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using Unity;

namespace eKnjiznica.AdminUI
{
    public partial class LoginForm : Form
    {
        private IUnityContainer UnityContainer;
        private IApiClient ApiClient;
        private IUserService userService;
        private ErrorHandlingUtil errorHandlingUtil;

        public LoginForm(IUnityContainer unityContainer, IApiClient apiClient,ErrorHandlingUtil errorHandlingUtil,IUserService userService)
        {
            this.UnityContainer = unityContainer;
            this.ApiClient = apiClient;
            this.errorHandlingUtil = errorHandlingUtil;
            this.userService = userService;
            InitializeComponent();
        }


        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            pbLoading.Visible = true;


            LoginVM loginVM = new LoginVM
            {
                Password = txtPassword.Text,
                Username = txtUsername.Text
            };
            var result = await this.ApiClient.LoginUser(loginVM,"wfa");

            if (result.IsSuccessStatusCode)
            {
                var authREsponse = await result.Content.ReadAsAsync<eKnjiznica.Commons.ViewModels.AuthenticationResponseVM>();
                userService.SaveAuthResponse(authREsponse);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = await errorHandlingUtil.GetLoginErrorMessage(result);
            }
            pbLoading.Visible = false;

        }
    }
}
