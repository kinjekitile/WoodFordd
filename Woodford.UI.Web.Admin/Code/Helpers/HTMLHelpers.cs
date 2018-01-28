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

namespace Woodford.UI.Web.Admin.Code.Helpers {
    public static class HTMLHelpers {

        public static MvcHtmlString FileUploadFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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
            //DefaultQueryProcessor queryProcessor = new DefaultQueryProcessor();
            FilesGetQuery query = new FilesGetQuery { Filter = new FileUploadFilterModel { } };


            ListOf<FileUploadModel> uploads = queryProcessor.Process(query);

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var r in uploads.Items) {
                var item = new SelectListItem();
                item.Text = r.Title.ToString();
                item.Value = r.Id.ToString();
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString CampaignFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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

            IQueryProcessor queryProcessor = MvcApplication.Container.GetInstance<IQueryProcessor>();
            //DefaultQueryProcessor queryProcessor = new DefaultQueryProcessor();
            CampaignGetQuery query = new CampaignGetQuery { Filter = new CampaignFilterModel { } };


            ListOf<CampaignModel> campaigns = queryProcessor.Process(query);

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var r in campaigns.Items) {
                var item = new SelectListItem();
                item.Text = r.Title.ToString();
                item.Value = r.Id.ToString();
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString RateCodeFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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

            IQueryProcessor queryProcessor = MvcApplication.Container.GetInstance<IQueryProcessor>();
            //DefaultQueryProcessor queryProcessor = new DefaultQueryProcessor();
            RateCodeGetQuery query = new RateCodeGetQuery { Filter = new RateCodeFilterModel { } };


            ListOf<RateCodeModel> rateCodes = queryProcessor.Process(query);

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var r in rateCodes.Items) {
                var item = new SelectListItem();
                item.Text = r.Title.ToString();
                item.Value = r.Id.ToString();
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString RateRuleFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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

            IQueryProcessor queryProcessor = MvcApplication.Container.GetInstance<IQueryProcessor>();
            //DefaultQueryProcessor queryProcessor = new DefaultQueryProcessor();
            RateRuleGetQuery query = new RateRuleGetQuery { Filter = new RateRuleFilterModel { } };
           

            ListOf<RateRuleModel> rateRules = queryProcessor.Process(query);

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var r in rateRules.Items) {
                var item = new SelectListItem();
                item.Text = r.Title + " min: " + r.MinDays + " max: " + r.MaxDays;
                item.Value = r.Id.ToString();
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString ReservationStateFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var reservationState in Enum.GetValues(typeof(ReservationState))) {
                var item = new SelectListItem();
                item.Text = ((ReservationState)reservationState).ToString();
                item.Value = Convert.ToString((int)reservationState);
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString VehicleGroupFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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
            //DefaultQueryProcessor queryProcessor = new DefaultQueryProcessor();
            VehicleGroupsGetQuery query = new VehicleGroupsGetQuery { Filter = new VehicleGroupFilterModel { IsArchived = false } };

            ListOf<VehicleGroupModel> vehicleGroups = queryProcessor.Process(query);

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var vg in vehicleGroups.Items) {
                var item = new SelectListItem();
                item.Text = vg.Title + " - " + vg.TitleDescription;
                item.Value = vg.Id.ToString();
                items.Add(item);
            }
           
            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString VehicleManufacaturerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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
            //DefaultQueryProcessor queryProcessor = new DefaultQueryProcessor();
            VehicleManufacturerGetQuery query = new VehicleManufacturerGetQuery { Filter = new VehicleManufacturerFilterModel {  } };

            ListOf<VehicleManufacturerModel> vehicleGroups = queryProcessor.Process(query);

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var vg in vehicleGroups.Items) {
                var item = new SelectListItem();
                item.Text = vg.Title;
                item.Value = vg.Id.ToString();
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString VehicleFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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
            //DefaultQueryProcessor queryProcessor = new DefaultQueryProcessor();
            VehiclesGetQuery query = new VehiclesGetQuery { Filter = new VehicleFilterModel { IsArchived = false } };

            ListOf<VehicleModel> vehicles = queryProcessor.Process(query);

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var vg in vehicles.Items) {
                var item = new SelectListItem();
                item.Text = vg.Title;
                item.Value = vg.Id.ToString();
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString RateTypeFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var rateType in Enum.GetValues(typeof(RateType))) {
                var item = new SelectListItem();
                item.Text = ((RateType)rateType).ToString();
                item.Value = Convert.ToString((int)rateType);
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

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
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var b in branches.Items) {
                var item = new SelectListItem();
                item.Text = b.Title.ToString();
                item.Value = b.Id.ToString();
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString CorporateFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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

            IQueryProcessor queryProcessor = MvcApplication.Container.GetInstance<IQueryProcessor>();
            CorporatesGetQuery query = new CorporatesGetQuery { Filter = new CorporateFilterModel { } };


            ListOf<CorporateModel> corps = queryProcessor.Process(query);

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var b in corps.Items) {
                var item = new SelectListItem();
                item.Text = b.Title.ToString();
                item.Value = b.Id.ToString();
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString VoucherRewardTypeFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var rewardType in Enum.GetValues(typeof(VoucherRewardType))) {
                var item = new SelectListItem();
                item.Text = ((VoucherRewardType)rewardType).ToString();
                item.Value = Convert.ToString((int)rewardType);
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString LoyaltyBenefitTierFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null, bool showAll = false) {
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
            
            var loyaltyTiers = EnumHelpers.GetValues<LoyaltyTierLevel>();

           
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            if (showAll) {
                var item = new SelectListItem();
                item.Text = LoyaltyTierLevel.All.GetDescription();
                item.Value = Convert.ToString((int)LoyaltyTierLevel.All);
                items.Add(item);
            }
            foreach (var tier in loyaltyTiers) {
                
                if (tier != LoyaltyTierLevel.All) {
                    var item = new SelectListItem();
                    item.Text = tier.GetDescription();
                    item.Value = Convert.ToString((int)tier);
                    items.Add(item);
                }
                
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString BenefitTypeFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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

            var benefitTypes = EnumHelpers.GetValues<BenefitType>();


            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var b in benefitTypes) {
                var item = new SelectListItem();
                item.Text = b.GetDescription();
                item.Value = Convert.ToString((int)b);
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }
        public static MvcHtmlString ReservationDateFilterTypeFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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

            var types = EnumHelpers.GetValues<ReservationDateFilterTypes>();


            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var b in types) {
                var item = new SelectListItem();
                item.Text = b.GetDescription();
                item.Value = Convert.ToString((int)b);
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString UserDateFilterTypeFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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

            var types = EnumHelpers.GetValues<UserDateFilterTypes>();


            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var b in types) {
                var item = new SelectListItem();
                item.Text = b.GetDescription();
                item.Value = Convert.ToString((int)b);
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }
        public static MvcHtmlString CountdownSpecialTypeFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var specialType in Enum.GetValues(typeof(CountdownSpecialType))) {
                var item = new SelectListItem();
                item.Text = ((CountdownSpecialType)specialType).ToString();
                item.Value = Convert.ToString((int)specialType);
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString VehicleUpgradeFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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
            VehicleUpgradesGetQuery query = new VehicleUpgradesGetQuery { Filter = new VehicleUpgradeFilterModel { IsActive = true } };


            ListOf<VehicleUpgradeModel> vehicleUpgrades = queryProcessor.Process(query);

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var v in vehicleUpgrades.Items) {
                var item = new SelectListItem();
                item.Text = v.FromVehicle.Title + " - " + v.ToVehicle.Title + " for " + v.Branch.Title;
                item.Value = v.Id.ToString();
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString RedirectTypeFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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

            var redirectTypes = EnumHelpers.GetValues<UrlRedirectType>();


            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var b in redirectTypes) {
                var item = new SelectListItem();
                item.Text = b.GetDescription();
                item.Value = Convert.ToString((int)b);
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString UserRoleFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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

            var roles = EnumHelpers.GetValues<UserRoles>();


            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var b in roles) {
                var item = new SelectListItem();
                item.Text = b.ToString();
                item.Value = Convert.ToString((int)b);
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString ReportDateUnitTypeFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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

            var roles = EnumHelpers.GetValues<ReportDateUnitType>();


            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "", Text = "Please Select" });
            foreach (var b in roles) {
                var item = new SelectListItem();
                item.Text = b.ToString();
                item.Value = Convert.ToString((int)b);
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString VehicleGroupTypeFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null, bool showAll = false) {
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

            var types = EnumHelpers.GetValues<VehicleGroupType>();


            List<SelectListItem> items = new List<SelectListItem>();
            
            if (showAll) {
                var item = new SelectListItem();
                item.Text = LoyaltyTierLevel.All.GetDescription();
                item.Value = Convert.ToString((int)LoyaltyTierLevel.All);
                items.Add(item);
            }
            foreach (var groupType in types) {

                
                    var item = new SelectListItem();
                    item.Text = groupType.GetDescription();
                    item.Value = Convert.ToString((int)groupType);
                    items.Add(item);
                

            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString NewsCategoryFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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
            NewsCategoryGetQuery query = new NewsCategoryGetQuery { Filter = new NewsCategoryFilterModel { } };


            ListOf<NewsCategoryModel> cats = queryProcessor.Process(query);

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var b in cats.Items) {
                var item = new SelectListItem();
                item.Text = b.Title.ToString();
                item.Value = b.Id.ToString();
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString UserSortFieldFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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

            var roles = EnumHelpers.GetValues<UserSortField>();


            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var b in roles) {
                var item = new SelectListItem();
                item.Text = b.ToString();
                item.Value = Convert.ToString((int)b);
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString UserSortDirectionFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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

            var roles = EnumHelpers.GetValues<UserSortDirection>();


            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var b in roles) {
                var item = new SelectListItem();
                item.Text = b.ToString();
                item.Value = Convert.ToString((int)b);
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }



        public static MvcHtmlString ReservationSortByFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
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
            //items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Please Select" });
            foreach (var reservationState in Enum.GetValues(typeof(ReservationSortByField))) {
                var item = new SelectListItem();
                item.Text = ((ReservationSortByField)reservationState).ToString();
                item.Value = Convert.ToString((int)reservationState);
                items.Add(item);
            }

            return html.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString ColorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null) {
            string propertyName = "";
            string currentPropertyValue = "";
            var memberSelectorExpression = expression.Body as MemberExpression;
            if (memberSelectorExpression != null) {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null) {
                    propertyName = property.Name;
                    try {
                        currentPropertyValue = Convert.ToString(expression.Compile().Invoke(html.ViewData.Model));
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
            items.Add(new SelectListItem { Selected = true, Value = "", Text = "Default" });
            items.Add(new SelectListItem { Selected = true, Value = "#b73537", Text = "Red" });
            items.Add(new SelectListItem { Selected = true, Value = "#19963c", Text = "Green" });


            foreach (var item in items) {
                if (item.Value == currentPropertyValue) {
                    item.Selected = true;
                } else {
                    item.Selected = false;
                }
            }
            return html.DropDownListFor(expression, items, htmlAttributes);
        }
    }
}
