using System.Web;
using System.Web.Optimization;

namespace WebApplication2
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{

			#region Scripts 
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));

			string[] AdminScripts = new string[]
			{
				"~/Scripts/jquery-1.10.2.min.js", 
				"~/Scripts/kendo.all.min.js",
				//might have to delete either the top kendo.js or the bottom two 
				"~/Scripts/kendo.aspnetmvc.min.js",
				"~/Scripts/kendo.web.min.js"
			};


            bundles.Add(new ScriptBundle("~/Content/js/admin_manager_chef_bundle")
				.Include(AdminScripts)
				.Include(
					"~/Content/Admin/Manager/Chefs.js"
				)
			);


            bundles.Add(new ScriptBundle("~/Content/js/admin_manager_deliveryman_bundle")
                .Include(AdminScripts)
                .Include(
                    "~/Content/Admin/Manager/Deliveryman.js"
                )
            );

            bundles.Add(new ScriptBundle("~/Content/js/admin_manager_user_bundle")
				.Include(AdminScripts)
				.Include(
					"~/Content/Admin/Manager/Users.js"
				)
			);


			bundles.Add(new ScriptBundle("~/Content/js/admin_chef_bundle")
				.Include(AdminScripts)
				.Include(
					"~/Content/Admin/Chef/Menu/Menu.js", 
					"~/Scripts/slim.jquery.min.js"
				)
			);

			bundles.Add(new ScriptBundle("~/Content/js/my_menu_bundle")
				.Include(
					"~/Scripts/knockout-3.4.2.js",
					"~/Content/Home/MyMenu.js"
				)
			);

			bundles.Add(new ScriptBundle("~/Content/js/menu_bundle")
				.Include(
					"~/Scripts/jquery-1.10.2.min.js",
					"~/Scripts/bootstrap.min.js",
					"~/Scripts/bootstrap-rating-input.min.js",
					"~/Scripts/knockout-3.4.2.js", 
					"~/Content/Home/Menu.js"
				)
			);

			bundles.Add(new ScriptBundle("~/Content/js/home_menu_bundle")
				.Include(
					"~/Scripts/jquery-1.10.2.min.js",
					"~/Scripts/bootstrap.min.js",
					"~/Scripts/bootstrap-rating-input.min.js",
					"~/Scripts/knockout-3.4.2.js"				
                    )
			);

			bundles.Add(new ScriptBundle("~/Content/js/item_bundle")
				.Include(
					"~/Scripts/jquery-1.10.2.min.js",
					"~/Scripts/bootstrap.min.js",
					"~/Scripts/bootstrap-rating-input.min.js",
					"~/Scripts/knockout-3.4.2.js",
					"~/Content/Item/Index.js"
				)
			);

			#endregion Scripts
			#region Styles 

			string[] AdminStyles = new string[]
			{
				"~/Content/kendo/kendo.common.min.css",
				"~/Content/kendo/kendo.default.min.css", 
				"~/Content/Slim/slim.min.css", 
			};


			bundles.Add(new StyleBundle("~/Content/css/admin_bundle")
				.Include(AdminStyles)
			);

            bundles.Add(new ScriptBundle("~/Content/js/admin_bundle")
                .Include(AdminScripts)
            );

            #endregion Styles

            BundleTable.EnableOptimizations = false;
		}
	}
}
