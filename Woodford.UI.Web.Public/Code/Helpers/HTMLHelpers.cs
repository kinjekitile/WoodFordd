using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Public.Code.Helpers {
    public static class HTMLHelpers {


        public static MvcHtmlString BranchFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
            string propertyName = "";
            int currentPropertyValue = 0;
            var memberSelectorExpression = expression.Body as MemberExpression;
            if (memberSelectorExpression != null) {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null) {
                    propertyName = property.Name;
                    try {
                        currentPropertyValue = Convert.ToInt32(expression.Compile().Invoke(html.ViewData.Model));
                    }
                    catch (Exception) { }

                }
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelValue = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            var name = html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

            ViewDataDictionary viewData = new ViewDataDictionary();
            viewData.Add("inputName", name);
            viewData.Add("inputId", id);
            viewData.Add("currentPropertyValue", currentPropertyValue);

            IQueryProcessor queryProcessor = MvcApplication.Container.GetInstance<IQueryProcessor>();
            BranchesGetQuery query = new BranchesGetQuery { Filter = new BranchFilterModel { IsArchived = false } };


            ListOf<BranchModel> branches = queryProcessor.Process(query);

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var b in branches.Items) {
                var item = new SelectListItem();
                item.Text = b.Title.ToString();
                item.Value = b.Id.ToString();
                if (b.Id == currentPropertyValue) {
                    item.Selected = true;
                }
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString BranchStringFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
            string propertyName = "";
            int currentPropertyValue = 0;
            var memberSelectorExpression = expression.Body as MemberExpression;
            if (memberSelectorExpression != null) {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null) {
                    propertyName = property.Name;
                    try {
                        currentPropertyValue = Convert.ToInt32(expression.Compile().Invoke(html.ViewData.Model));
                    }
                    catch (Exception) { }

                }
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelValue = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            var name = html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

            ViewDataDictionary viewData = new ViewDataDictionary();
            viewData.Add("inputName", name);
            viewData.Add("inputId", id);
            viewData.Add("currentPropertyValue", currentPropertyValue);

            IQueryProcessor queryProcessor = MvcApplication.Container.GetInstance<IQueryProcessor>();
            BranchesGetQuery query = new BranchesGetQuery { Filter = new BranchFilterModel { IsArchived = false } };


            ListOf<BranchModel> branches = queryProcessor.Process(query);

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var b in branches.Items) {
                var item = new SelectListItem();
                item.Text = b.Title.ToString();
                item.Value = b.Title.ToString();
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString BranchDisplayFor(this HtmlHelper html, int id) {

            IQueryProcessor queryProcessor = MvcApplication.Container.GetInstance<IQueryProcessor>();
            BranchGetByIdQuery query = new BranchGetByIdQuery { Id = id, IncludePageContent = false };
            return new MvcHtmlString(queryProcessor.Process(query).Title);
        }

        public static MvcHtmlString HourOfDayDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
            string propertyName = "";
            int currentPropertyValue = 0;
            var memberSelectorExpression = expression.Body as MemberExpression;
            if (memberSelectorExpression != null) {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null) {
                    propertyName = property.Name;
                    try {
                        currentPropertyValue = Convert.ToInt32(expression.Compile().Invoke(html.ViewData.Model));
                    }
                    catch (Exception) { }

                }
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelValue = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            var name = html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

            ViewDataDictionary viewData = new ViewDataDictionary();
            viewData.Add("inputName", name);
            viewData.Add("inputId", id);
            viewData.Add("currentPropertyValue", currentPropertyValue);

            List<SelectListItem> items = new List<SelectListItem>();

            

            for (int i = 7; i < 12; i++) {
                var item = new SelectListItem();
                item.Text = i.ToString() + ":00 AM";
                item.Value = i.ToString();
                items.Add(item);
            }

            var itemTwelve = new SelectListItem();
            itemTwelve.Text = "12:00 PM";
            itemTwelve.Value = "12";
            items.Add(itemTwelve);

            for (int i = 1; i < 11; i++) {
                var item = new SelectListItem();
                item.Text = i.ToString() + ":00 PM";
                item.Value = (i + 12).ToString();
                items.Add(item);
            }

            //for (int i = 1; i < 3; i++) {
            //    var item = new SelectListItem();
            //    item.Text = i.ToString() + ":00 AM";
            //    item.Value = i.ToString();
            //    items.Add(item);
            //}

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString DatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
            string propertyName = "";
            DateTime currentPropertyValue = DateTime.Today.Date;
            var memberSelectorExpression = expression.Body as MemberExpression;
            if (memberSelectorExpression != null) {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null) {
                    propertyName = property.Name;
                    try {
                        currentPropertyValue = Convert.ToDateTime(expression.Compile().Invoke(html.ViewData.Model));
                    }
                    catch (Exception) { }

                }
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelValue = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();


            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            var name = html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

            return new MvcHtmlString("<input type=\"text\" id=\"" + id + "\" name=\"" + name + "\" value=\"" + currentPropertyValue.Date.ToString("yyyy-MM-dd") + "\" class=\"datefield\" readonly />");

        }

        public static MvcHtmlString CountryDropListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select Country", Value = "", Selected = false });
            items.Add(new SelectListItem { Text = "Afghanistan", Value = "Afghanistan", Selected = false });
            items.Add(new SelectListItem { Text = "Albania", Value = "Albania", Selected = false });
            items.Add(new SelectListItem { Text = "Algeria", Value = "Algeria", Selected = false });
            items.Add(new SelectListItem { Text = "American Samoa", Value = "American Samoa", Selected = false });
            items.Add(new SelectListItem { Text = "Andorra", Value = "Andorra", Selected = false });
            items.Add(new SelectListItem { Text = "Angola", Value = "Angola", Selected = false });
            items.Add(new SelectListItem { Text = "Anguilla", Value = "Anguilla", Selected = false });
            items.Add(new SelectListItem { Text = "Antarctica", Value = "Antarctica", Selected = false });
            items.Add(new SelectListItem { Text = "Antigua And Barbuda", Value = "Antigua And Barbuda", Selected = false });
            items.Add(new SelectListItem { Text = "Argentina", Value = "Argentina", Selected = false });
            items.Add(new SelectListItem { Text = "Armenia", Value = "Armenia", Selected = false });
            items.Add(new SelectListItem { Text = "Aruba", Value = "Aruba", Selected = false });
            items.Add(new SelectListItem { Text = "Australia", Value = "Australia", Selected = false });
            items.Add(new SelectListItem { Text = "Austria", Value = "Austria", Selected = false });
            items.Add(new SelectListItem { Text = "Azerbaijan", Value = "Azerbaijan", Selected = false });
            items.Add(new SelectListItem { Text = "Bahamas", Value = "Bahamas", Selected = false });
            items.Add(new SelectListItem { Text = "Bahrain", Value = "Bahrain", Selected = false });
            items.Add(new SelectListItem { Text = "Bangladesh", Value = "Bangladesh", Selected = false });
            items.Add(new SelectListItem { Text = "Barbados", Value = "Barbados", Selected = false });
            items.Add(new SelectListItem { Text = "Belarus", Value = "Belarus", Selected = false });
            items.Add(new SelectListItem { Text = "Belgium", Value = "Belgium", Selected = false });
            items.Add(new SelectListItem { Text = "Belize", Value = "Belize", Selected = false });
            items.Add(new SelectListItem { Text = "Benin", Value = "Benin", Selected = false });
            items.Add(new SelectListItem { Text = "Bermuda", Value = "Bermuda", Selected = false });
            items.Add(new SelectListItem { Text = "Bhutan", Value = "Bhutan", Selected = false });
            items.Add(new SelectListItem { Text = "Bolivia", Value = "Bolivia", Selected = false });
            items.Add(new SelectListItem { Text = "Bosnia And Herzegowina", Value = "Bosnia And Herzegowina", Selected = false });
            items.Add(new SelectListItem { Text = "Botswana", Value = "Botswana", Selected = false });
            items.Add(new SelectListItem { Text = "Bouvet Island", Value = "Bouvet Island", Selected = false });
            items.Add(new SelectListItem { Text = "Brazil", Value = "Brazil", Selected = false });
            items.Add(new SelectListItem { Text = "British Indian Ocean Territory", Value = "British Indian Ocean Territory", Selected = false });
            items.Add(new SelectListItem { Text = "Brunei Darussalam", Value = "Brunei Darussalam", Selected = false });
            items.Add(new SelectListItem { Text = "Bulgaria", Value = "Bulgaria", Selected = false });
            items.Add(new SelectListItem { Text = "Burkina Faso", Value = "Burkina Faso", Selected = false });
            items.Add(new SelectListItem { Text = "Burundi", Value = "Burundi", Selected = false });
            items.Add(new SelectListItem { Text = "Cambodia", Value = "Cambodia", Selected = false });
            items.Add(new SelectListItem { Text = "Cameroon", Value = "Cameroon", Selected = false });
            items.Add(new SelectListItem { Text = "Canada", Value = "Canada", Selected = false });
            items.Add(new SelectListItem { Text = "Cape Verde", Value = "Cape Verde", Selected = false });
            items.Add(new SelectListItem { Text = "Cayman Islands", Value = "Cayman Islands", Selected = false });
            items.Add(new SelectListItem { Text = "Central African Republic", Value = "Central African Republic", Selected = false });
            items.Add(new SelectListItem { Text = "Chad", Value = "Chad", Selected = false });
            items.Add(new SelectListItem { Text = "Chile", Value = "Chile", Selected = false });
            items.Add(new SelectListItem { Text = "China", Value = "China", Selected = false });
            items.Add(new SelectListItem { Text = "Christmas Island", Value = "Christmas Island", Selected = false });
            items.Add(new SelectListItem { Text = "Cocos (Keeling) Islands", Value = "Cocos (Keeling) Islands", Selected = false });
            items.Add(new SelectListItem { Text = "Colombia", Value = "Colombia", Selected = false });
            items.Add(new SelectListItem { Text = "Comoros", Value = "Comoros", Selected = false });
            items.Add(new SelectListItem { Text = "Congo", Value = "Congo", Selected = false });
            items.Add(new SelectListItem { Text = "Cook Islands", Value = "Cook Islands", Selected = false });
            items.Add(new SelectListItem { Text = "Costa Rica", Value = "Costa Rica", Selected = false });
            items.Add(new SelectListItem { Text = "Cote D Ivoire", Value = "Cote D Ivoire", Selected = false });
            items.Add(new SelectListItem { Text = "Croatia (Local Name: Hrvatska)", Value = "Croatia (Local Name: Hrvatska)", Selected = false });
            items.Add(new SelectListItem { Text = "Cuba", Value = "Cuba", Selected = false });
            items.Add(new SelectListItem { Text = "Cyprus", Value = "Cyprus", Selected = false });
            items.Add(new SelectListItem { Text = "Czech Republic", Value = "Czech Republic", Selected = false });
            items.Add(new SelectListItem { Text = "Denmark", Value = "Denmark", Selected = false });
            items.Add(new SelectListItem { Text = "Djibouti", Value = "Djibouti", Selected = false });
            items.Add(new SelectListItem { Text = "Dominica", Value = "Dominica", Selected = false });
            items.Add(new SelectListItem { Text = "Dominican Republic", Value = "Dominican Republic", Selected = false });
            items.Add(new SelectListItem { Text = "East Timor", Value = "East Timor", Selected = false });
            items.Add(new SelectListItem { Text = "Ecuador", Value = "Ecuador", Selected = false });
            items.Add(new SelectListItem { Text = "Egypt", Value = "Egypt", Selected = false });
            items.Add(new SelectListItem { Text = "El Salvador", Value = "El Salvador", Selected = false });
            items.Add(new SelectListItem { Text = "Equatorial Guinea", Value = "Equatorial Guinea", Selected = false });
            items.Add(new SelectListItem { Text = "Eritrea", Value = "Eritrea", Selected = false });
            items.Add(new SelectListItem { Text = "Estonia", Value = "Estonia", Selected = false });
            items.Add(new SelectListItem { Text = "Ethiopia", Value = "Ethiopia", Selected = false });
            items.Add(new SelectListItem { Text = "Falkland Islands (Malvinas)", Value = "Falkland Islands (Malvinas)", Selected = false });
            items.Add(new SelectListItem { Text = "Faroe Islands", Value = "Faroe Islands", Selected = false });
            items.Add(new SelectListItem { Text = "Fiji", Value = "Fiji", Selected = false });
            items.Add(new SelectListItem { Text = "Finland", Value = "Finland", Selected = false });
            items.Add(new SelectListItem { Text = "France", Value = "France", Selected = false });
            items.Add(new SelectListItem { Text = "French Guiana", Value = "French Guiana", Selected = false });
            items.Add(new SelectListItem { Text = "French Polynesia", Value = "French Polynesia", Selected = false });
            items.Add(new SelectListItem { Text = "French Southern Territories", Value = "French Southern Territories", Selected = false });
            items.Add(new SelectListItem { Text = "Gabon", Value = "Gabon", Selected = false });
            items.Add(new SelectListItem { Text = "Gambia", Value = "Gambia", Selected = false });
            items.Add(new SelectListItem { Text = "Georgia", Value = "Georgia", Selected = false });
            items.Add(new SelectListItem { Text = "Germany", Value = "Germany", Selected = false });
            items.Add(new SelectListItem { Text = "Ghana", Value = "Ghana", Selected = false });
            items.Add(new SelectListItem { Text = "Gibraltar", Value = "Gibraltar", Selected = false });
            items.Add(new SelectListItem { Text = "Greece", Value = "Greece", Selected = false });
            items.Add(new SelectListItem { Text = "Greenland", Value = "Greenland", Selected = false });
            items.Add(new SelectListItem { Text = "Grenada", Value = "Grenada", Selected = false });
            items.Add(new SelectListItem { Text = "Guadeloupe", Value = "Guadeloupe", Selected = false });
            items.Add(new SelectListItem { Text = "Guam", Value = "Guam", Selected = false });
            items.Add(new SelectListItem { Text = "Guatemala", Value = "Guatemala", Selected = false });
            items.Add(new SelectListItem { Text = "Guinea", Value = "Guinea", Selected = false });
            items.Add(new SelectListItem { Text = "Guinea-Bissau", Value = "Guinea-Bissau", Selected = false });
            items.Add(new SelectListItem { Text = "Guyana", Value = "Guyana", Selected = false });
            items.Add(new SelectListItem { Text = "Haiti", Value = "Haiti", Selected = false });
            items.Add(new SelectListItem { Text = "Heard And Mc Donald Islands", Value = "Heard And Mc Donald Islands", Selected = false });
            items.Add(new SelectListItem { Text = "Holy See (Vatican City State)", Value = "Holy See (Vatican City State)", Selected = false });
            items.Add(new SelectListItem { Text = "Honduras", Value = "Honduras", Selected = false });
            items.Add(new SelectListItem { Text = "Hong Kong", Value = "Hong Kong", Selected = false });
            items.Add(new SelectListItem { Text = "Hungary", Value = "Hungary", Selected = false });
            items.Add(new SelectListItem { Text = "Icel And", Value = "Icel And", Selected = false });
            items.Add(new SelectListItem { Text = "India", Value = "India", Selected = false });
            items.Add(new SelectListItem { Text = "Indonesia", Value = "Indonesia", Selected = false });
            items.Add(new SelectListItem { Text = "Iran (Islamic Republic Of)", Value = "Iran (Islamic Republic Of)", Selected = false });
            items.Add(new SelectListItem { Text = "Iraq", Value = "Iraq", Selected = false });
            items.Add(new SelectListItem { Text = "Ireland", Value = "Ireland", Selected = false });
            items.Add(new SelectListItem { Text = "Israel", Value = "Israel", Selected = false });
            items.Add(new SelectListItem { Text = "Italy", Value = "Italy", Selected = false });
            items.Add(new SelectListItem { Text = "Jamaica", Value = "Jamaica", Selected = false });
            items.Add(new SelectListItem { Text = "Japan", Value = "Japan", Selected = false });
            items.Add(new SelectListItem { Text = "Jordan", Value = "Jordan", Selected = false });
            items.Add(new SelectListItem { Text = "Kazakhstan", Value = "Kazakhstan", Selected = false });
            items.Add(new SelectListItem { Text = "Kenya", Value = "Kenya", Selected = false });
            items.Add(new SelectListItem { Text = "Kiribati", Value = "Kiribati", Selected = false });
            items.Add(new SelectListItem { Text = "Korea, Dem People&#39;S Republic", Value = "Korea, Dem People&#39;S Republic", Selected = false });
            items.Add(new SelectListItem { Text = "Korea, Republic Of", Value = "Korea, Republic Of", Selected = false });
            items.Add(new SelectListItem { Text = "Kuwait", Value = "Kuwait", Selected = false });
            items.Add(new SelectListItem { Text = "Kyrgyzstan", Value = "Kyrgyzstan", Selected = false });
            items.Add(new SelectListItem { Text = "Lao People&#39;S Dem Republic", Value = "Lao People&#39;S Dem Republic", Selected = false });
            items.Add(new SelectListItem { Text = "Latvia", Value = "Latvia", Selected = false });
            items.Add(new SelectListItem { Text = "Lebanon", Value = "Lebanon", Selected = false });
            items.Add(new SelectListItem { Text = "Lesotho", Value = "Lesotho", Selected = false });
            items.Add(new SelectListItem { Text = "Liberia", Value = "Liberia", Selected = false });
            items.Add(new SelectListItem { Text = "Libyan Arab Jamahiriya", Value = "Libyan Arab Jamahiriya", Selected = false });
            items.Add(new SelectListItem { Text = "Liechtenstein", Value = "Liechtenstein", Selected = false });
            items.Add(new SelectListItem { Text = "Lithuania", Value = "Lithuania", Selected = false });
            items.Add(new SelectListItem { Text = "Luxembourg", Value = "Luxembourg", Selected = false });
            items.Add(new SelectListItem { Text = "Macau", Value = "Macau", Selected = false });
            items.Add(new SelectListItem { Text = "Macedonia", Value = "Macedonia", Selected = false });
            items.Add(new SelectListItem { Text = "Madagascar", Value = "Madagascar", Selected = false });
            items.Add(new SelectListItem { Text = "Malawi", Value = "Malawi", Selected = false });
            items.Add(new SelectListItem { Text = "Malaysia", Value = "Malaysia", Selected = false });
            items.Add(new SelectListItem { Text = "Maldives", Value = "Maldives", Selected = false });
            items.Add(new SelectListItem { Text = "Mali", Value = "Mali", Selected = false });
            items.Add(new SelectListItem { Text = "Malta", Value = "Malta", Selected = false });
            items.Add(new SelectListItem { Text = "Marshall Islands", Value = "Marshall Islands", Selected = false });
            items.Add(new SelectListItem { Text = "Martinique", Value = "Martinique", Selected = false });
            items.Add(new SelectListItem { Text = "Mauritania", Value = "Mauritania", Selected = false });
            items.Add(new SelectListItem { Text = "Mauritius", Value = "Mauritius", Selected = false });
            items.Add(new SelectListItem { Text = "Mayotte", Value = "Mayotte", Selected = false });
            items.Add(new SelectListItem { Text = "Mexico", Value = "Mexico", Selected = false });
            items.Add(new SelectListItem { Text = "Micronesia, Federated States", Value = "Micronesia, Federated States", Selected = false });
            items.Add(new SelectListItem { Text = "Moldova, Republic Of", Value = "Moldova, Republic Of", Selected = false });
            items.Add(new SelectListItem { Text = "Monaco", Value = "Monaco", Selected = false });
            items.Add(new SelectListItem { Text = "Mongolia", Value = "Mongolia", Selected = false });
            items.Add(new SelectListItem { Text = "Montserrat", Value = "Montserrat", Selected = false });
            items.Add(new SelectListItem { Text = "Morocco", Value = "Morocco", Selected = false });
            items.Add(new SelectListItem { Text = "Mozambique", Value = "Mozambique", Selected = false });
            items.Add(new SelectListItem { Text = "Myanmar", Value = "Myanmar", Selected = false });
            items.Add(new SelectListItem { Text = "Namibia", Value = "Namibia", Selected = false });
            items.Add(new SelectListItem { Text = "Nauru", Value = "Nauru", Selected = false });
            items.Add(new SelectListItem { Text = "Nepal", Value = "Nepal", Selected = false });
            items.Add(new SelectListItem { Text = "Netherlands", Value = "Netherlands", Selected = false });
            items.Add(new SelectListItem { Text = "Netherlands Ant Illes", Value = "Netherlands Ant Illes", Selected = false });
            items.Add(new SelectListItem { Text = "New Caledonia", Value = "New Caledonia", Selected = false });
            items.Add(new SelectListItem { Text = "New Zealand", Value = "New Zealand", Selected = false });
            items.Add(new SelectListItem { Text = "Nicaragua", Value = "Nicaragua", Selected = false });
            items.Add(new SelectListItem { Text = "Niger", Value = "Niger", Selected = false });
            items.Add(new SelectListItem { Text = "Nigeria", Value = "Nigeria", Selected = false });
            items.Add(new SelectListItem { Text = "Niue", Value = "Niue", Selected = false });
            items.Add(new SelectListItem { Text = "Norfolk Island", Value = "Norfolk Island", Selected = false });
            items.Add(new SelectListItem { Text = "Northern Mariana Islands", Value = "Northern Mariana Islands", Selected = false });
            items.Add(new SelectListItem { Text = "Norway", Value = "Norway", Selected = false });
            items.Add(new SelectListItem { Text = "Oman", Value = "Oman", Selected = false });
            items.Add(new SelectListItem { Text = "Pakistan", Value = "Pakistan", Selected = false });
            items.Add(new SelectListItem { Text = "Palau", Value = "Palau", Selected = false });
            items.Add(new SelectListItem { Text = "Panama", Value = "Panama", Selected = false });
            items.Add(new SelectListItem { Text = "Papua New Guinea", Value = "Papua New Guinea", Selected = false });
            items.Add(new SelectListItem { Text = "Paraguay", Value = "Paraguay", Selected = false });
            items.Add(new SelectListItem { Text = "Peru", Value = "Peru", Selected = false });
            items.Add(new SelectListItem { Text = "Philippines", Value = "Philippines", Selected = false });
            items.Add(new SelectListItem { Text = "Pitcairn", Value = "Pitcairn", Selected = false });
            items.Add(new SelectListItem { Text = "Poland", Value = "Poland", Selected = false });
            items.Add(new SelectListItem { Text = "Portugal", Value = "Portugal", Selected = false });
            items.Add(new SelectListItem { Text = "Puerto Rico", Value = "Puerto Rico", Selected = false });
            items.Add(new SelectListItem { Text = "Qatar", Value = "Qatar", Selected = false });
            items.Add(new SelectListItem { Text = "Reunion", Value = "Reunion", Selected = false });
            items.Add(new SelectListItem { Text = "Romania", Value = "Romania", Selected = false });
            items.Add(new SelectListItem { Text = "Russian Federation", Value = "Russian Federation", Selected = false });
            items.Add(new SelectListItem { Text = "Rwanda", Value = "Rwanda", Selected = false });
            items.Add(new SelectListItem { Text = "Saint K Itts And Nevis", Value = "Saint K Itts And Nevis", Selected = false });
            items.Add(new SelectListItem { Text = "Saint Lucia", Value = "Saint Lucia", Selected = false });
            items.Add(new SelectListItem { Text = "Saint Vincent, The Grenadines", Value = "Saint Vincent, The Grenadines", Selected = false });
            items.Add(new SelectListItem { Text = "Samoa", Value = "Samoa", Selected = false });
            items.Add(new SelectListItem { Text = "San Marino", Value = "San Marino", Selected = false });
            items.Add(new SelectListItem { Text = "Sao Tome And Principe", Value = "Sao Tome And Principe", Selected = false });
            items.Add(new SelectListItem { Text = "Saudi Arabia", Value = "Saudi Arabia", Selected = false });
            items.Add(new SelectListItem { Text = "Senegal", Value = "Senegal", Selected = false });
            items.Add(new SelectListItem { Text = "Seychelles", Value = "Seychelles", Selected = false });
            items.Add(new SelectListItem { Text = "Sierra Leone", Value = "Sierra Leone", Selected = false });
            items.Add(new SelectListItem { Text = "Singapore", Value = "Singapore", Selected = false });
            items.Add(new SelectListItem { Text = "Slovakia (Slovak Republic)", Value = "Slovakia (Slovak Republic)", Selected = false });
            items.Add(new SelectListItem { Text = "Slovenia", Value = "Slovenia", Selected = false });
            items.Add(new SelectListItem { Text = "Solomon Islands", Value = "Solomon Islands", Selected = false });
            items.Add(new SelectListItem { Text = "Somalia", Value = "Somalia", Selected = false });
            items.Add(new SelectListItem { Text = "South Africa", Value = "South Africa", Selected = true });
            items.Add(new SelectListItem { Text = "South Georgia , S Sandwich Is.", Value = "South Georgia , S Sandwich Is.", Selected = false });
            items.Add(new SelectListItem { Text = "Spain", Value = "Spain", Selected = false });
            items.Add(new SelectListItem { Text = "Sri Lanka", Value = "Sri Lanka", Selected = false });
            items.Add(new SelectListItem { Text = "St. Helena", Value = "St. Helena", Selected = false });
            items.Add(new SelectListItem { Text = "St. Pierre And Miquelon", Value = "St. Pierre And Miquelon", Selected = false });
            items.Add(new SelectListItem { Text = "Sudan", Value = "Sudan", Selected = false });
            items.Add(new SelectListItem { Text = "Suriname", Value = "Suriname", Selected = false });
            items.Add(new SelectListItem { Text = "Svalbard, Jan Mayen Islands", Value = "Svalbard, Jan Mayen Islands", Selected = false });
            items.Add(new SelectListItem { Text = "Sw Aziland", Value = "Sw Aziland", Selected = false });
            items.Add(new SelectListItem { Text = "Sweden", Value = "Sweden", Selected = false });
            items.Add(new SelectListItem { Text = "Switzerland", Value = "Switzerland", Selected = false });
            items.Add(new SelectListItem { Text = "Syrian Arab Republic", Value = "Syrian Arab Republic", Selected = false });
            items.Add(new SelectListItem { Text = "Taiwan", Value = "Taiwan", Selected = false });
            items.Add(new SelectListItem { Text = "Tajikistan", Value = "Tajikistan", Selected = false });
            items.Add(new SelectListItem { Text = "Tanzania, United Republic Of", Value = "Tanzania, United Republic Of", Selected = false });
            items.Add(new SelectListItem { Text = "Thailand", Value = "Thailand", Selected = false });
            items.Add(new SelectListItem { Text = "Togo", Value = "Togo", Selected = false });
            items.Add(new SelectListItem { Text = "Tokelau", Value = "Tokelau", Selected = false });
            items.Add(new SelectListItem { Text = "Tonga", Value = "Tonga", Selected = false });
            items.Add(new SelectListItem { Text = "Trinidad And Tobago", Value = "Trinidad And Tobago", Selected = false });
            items.Add(new SelectListItem { Text = "Tunisia", Value = "Tunisia", Selected = false });
            items.Add(new SelectListItem { Text = "Turkey", Value = "Turkey", Selected = false });
            items.Add(new SelectListItem { Text = "Turkmenistan", Value = "Turkmenistan", Selected = false });
            items.Add(new SelectListItem { Text = "Turks And Caicos Islands", Value = "Turks And Caicos Islands", Selected = false });
            items.Add(new SelectListItem { Text = "Tuvalu", Value = "Tuvalu", Selected = false });
            items.Add(new SelectListItem { Text = "Uganda", Value = "Uganda", Selected = false });
            items.Add(new SelectListItem { Text = "Ukraine", Value = "Ukraine", Selected = false });
            items.Add(new SelectListItem { Text = "United Arab Emirates", Value = "United Arab Emirates", Selected = false });
            items.Add(new SelectListItem { Text = "United Kingdom", Value = "United Kingdom", Selected = false });
            items.Add(new SelectListItem { Text = "United States", Value = "United States", Selected = false });
            items.Add(new SelectListItem { Text = "United States Minor Is.", Value = "United States Minor Is.", Selected = false });
            items.Add(new SelectListItem { Text = "Uruguay", Value = "Uruguay", Selected = false });
            items.Add(new SelectListItem { Text = "Uzbekistan", Value = "Uzbekistan", Selected = false });
            items.Add(new SelectListItem { Text = "Vanuatu", Value = "Vanuatu", Selected = false });
            items.Add(new SelectListItem { Text = "Venezuela", Value = "Venezuela", Selected = false });
            items.Add(new SelectListItem { Text = "Viet Nam", Value = "Viet Nam", Selected = false });
            items.Add(new SelectListItem { Text = "Virgin Islands (British)", Value = "Virgin Islands (British)", Selected = false });
            items.Add(new SelectListItem { Text = "Virgin Islands (U.S.)", Value = "Virgin Islands (U.S.)", Selected = false });
            items.Add(new SelectListItem { Text = "Wallis And Futuna Islands", Value = "Wallis And Futuna Islands", Selected = false });
            items.Add(new SelectListItem { Text = "Western Sahara", Value = "Western Sahara", Selected = false });
            items.Add(new SelectListItem { Text = "Yemen", Value = "Yemen", Selected = false });
            items.Add(new SelectListItem { Text = "Yugoslavia", Value = "Yugoslavia", Selected = false });
            items.Add(new SelectListItem { Text = "Zaire", Value = "Zaire", Selected = false });
            items.Add(new SelectListItem { Text = "Zambia", Value = "Zambia", Selected = false });
            items.Add(new SelectListItem { Text = "Zimbabwe", Value = "Zimbabwe", Selected = false });


            return html.DropDownListFor(expression, items, htmlAttributes);

        }

        public static MvcHtmlString CardExpiryYearFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
            string propertyName = "";
            int currentPropertyValue = 0;
            var memberSelectorExpression = expression.Body as MemberExpression;
            if (memberSelectorExpression != null) {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null) {
                    propertyName = property.Name;
                    try {
                        currentPropertyValue = Convert.ToInt32(expression.Compile().Invoke(html.ViewData.Model));
                    } catch (Exception) { }

                }
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelValue = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();


            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            var name = html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

            ViewDataDictionary viewData = new ViewDataDictionary();
            viewData.Add("inputName", name);
            viewData.Add("inputId", id);
            viewData.Add("currentPropertyValue", currentPropertyValue);

            int yearStart = DateTime.Today.Year;
            int yearsAhead = yearStart + 10;

            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = yearStart; i < yearsAhead; i++) {
                var item = new SelectListItem();
                item.Text = i.ToString();
                item.Value = i.ToString();
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString CardExpiryMonthFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
            string propertyName = "";
            int currentPropertyValue = 0;
            var memberSelectorExpression = expression.Body as MemberExpression;
            if (memberSelectorExpression != null) {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null) {
                    propertyName = property.Name;
                    try {
                        currentPropertyValue = Convert.ToInt32(expression.Compile().Invoke(html.ViewData.Model));
                    } catch (Exception) { }

                }
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelValue = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();


            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            var name = html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

            ViewDataDictionary viewData = new ViewDataDictionary();
            viewData.Add("inputName", name);
            viewData.Add("inputId", id);
            viewData.Add("currentPropertyValue", currentPropertyValue);



            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 1; i < 13; i++) {
                var item = new SelectListItem();
                item.Text = i.ToString();
                item.Value = i.ToString().Length == 1 ? "0" + i.ToString() : i.ToString();
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString CardTypeRadioButtonListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
            string propertyName = "";
            int currentPropertyValue = 0;
            var memberSelectorExpression = expression.Body as MemberExpression;
            if (memberSelectorExpression != null) {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null) {
                    propertyName = property.Name;
                    try {
                        currentPropertyValue = Convert.ToInt32(expression.Compile().Invoke(html.ViewData.Model));
                    } catch (Exception) { }

                }
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelValue = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();


            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            var name = html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

            ViewDataDictionary viewData = new ViewDataDictionary();
            viewData.Add("inputName", name);
            viewData.Add("inputId", id);
            viewData.Add("currentPropertyValue", currentPropertyValue);

            html.RenderPartial("_CardTypeSelector", null, viewData);

            return new MvcHtmlString("");
        }


    }
}
