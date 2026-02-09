using System.Globalization;

namespace pets.Security
{
    public static class PermissionTranslator
    {
        public static string ToDisplay(string permission)
        {
            var isAr = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

            return permission switch
            {
                Permissions.Dashboard_View => isAr ? "لوحة التحكم - عرض" : "Dashboard - View",

                Permissions.Appointments_View   => isAr ? "المواعيد - عرض" : "Appointments - View",
                Permissions.Appointments_Create => isAr ? "المواعيد - إضافة" : "Appointments - Create",
                Permissions.Appointments_Edit   => isAr ? "المواعيد - تعديل" : "Appointments - Edit",
                Permissions.Appointments_Delete => isAr ? "المواعيد - حذف" : "Appointments - Delete",

                Permissions.Pets_View   => isAr ? "الحيوانات الأليفة - عرض" : "Pets - View",
                Permissions.Pets_Create => isAr ? "الحيوانات الأليفة - إضافة" : "Pets - Create",
                Permissions.Pets_Edit   => isAr ? "الحيوانات الأليفة - تعديل" : "Pets - Edit",
                Permissions.Pets_Delete => isAr ? "الحيوانات الأليفة - حذف" : "Pets - Delete",

                Permissions.Owners_View   => isAr ? "الملاك - عرض" : "Owners - View",
                Permissions.Owners_Create => isAr ? "الملاك - إضافة" : "Owners - Create",
                Permissions.Owners_Edit   => isAr ? "الملاك - تعديل" : "Owners - Edit",
                Permissions.Owners_Delete => isAr ? "الملاك - حذف" : "Owners - Delete",

                Permissions.Inventory_View   => isAr ? "المخزون - عرض" : "Inventory - View",
                Permissions.Inventory_Create => isAr ? "المخزون - إضافة" : "Inventory - Create",
                Permissions.Inventory_Edit   => isAr ? "المخزون - تعديل" : "Inventory - Edit",
                Permissions.Inventory_Delete => isAr ? "المخزون - حذف" : "Inventory - Delete",

                Permissions.Reports_View => isAr ? "التقارير - عرض" : "Reports - View",

                Permissions.Settings_View => isAr ? "الإعدادات - عرض" : "Settings - View",
                Permissions.Settings_Edit => isAr ? "الإعدادات - تعديل" : "Settings - Edit",

                Permissions.Tools_View   => isAr ? "الأدوات - عرض" : "Tools - View",
                Permissions.Tools_Create => isAr ? "الأدوات - إضافة" : "Tools - Create",
                Permissions.Tools_Edit   => isAr ? "الأدوات - تعديل" : "Tools - Edit",
                Permissions.Tools_Delete => isAr ? "الأدوات - حذف" : "Tools - Delete",

                Permissions.Roles_View => isAr ? "الأدوار - عرض" : "Roles - View",
                Permissions.Roles_Edit => isAr ? "الأدوار - تعديل" : "Roles - Edit",

                _ => isAr
                    ? permission.Replace("Permissions.", "").Replace(".", " - ")
                    : permission.Replace("Permissions.", "").Replace(".", " - ")
            };
        }
    }
}
