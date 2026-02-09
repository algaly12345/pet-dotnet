namespace pets.Security
{
    public static class Permissions
    {
        // Dashboard
        public const string Dashboard_View = "Permissions.Dashboard.View";

        // Appointments
        public const string Appointments_View = "Permissions.Appointments.View";
        public const string Appointments_Create = "Permissions.Appointments.Create";
        public const string Appointments_Edit = "Permissions.Appointments.Edit";
        public const string Appointments_Delete = "Permissions.Appointments.Delete";

        // Pets
        public const string Pets_View = "Permissions.Pets.View";
        public const string Pets_Create = "Permissions.Pets.Create";
        public const string Pets_Edit = "Permissions.Pets.Edit";
        public const string Pets_Delete = "Permissions.Pets.Delete";

        // Owners
        public const string Owners_View = "Permissions.Owners.View";
        public const string Owners_Create = "Permissions.Owners.Create";
        public const string Owners_Edit = "Permissions.Owners.Edit";
        public const string Owners_Delete = "Permissions.Owners.Delete";

        // Inventory
        public const string Inventory_View = "Permissions.Inventory.View";
        public const string Inventory_Create = "Permissions.Inventory.Create";
        public const string Inventory_Edit = "Permissions.Inventory.Edit";
        public const string Inventory_Delete = "Permissions.Inventory.Delete";

        // Reports
        public const string Reports_View = "Permissions.Reports.View";

        // Settings
        public const string Settings_View = "Permissions.Settings.View";
        public const string Settings_Edit = "Permissions.Settings.Edit";



        // Tools
public const string Tools_View   = "Permissions.Tools.View";
public const string Tools_Create = "Permissions.Tools.Create";
public const string Tools_Edit   = "Permissions.Tools.Edit";
public const string Tools_Delete = "Permissions.Tools.Delete";


        // (اختياري) Admin Roles Management
        public const string Roles_View = "Permissions.Roles.View";
        public const string Roles_Edit = "Permissions.Roles.Edit";

        public static readonly string[] All = new[]
        {
             Tools_View, Tools_Create, Tools_Edit, Tools_Delete,
            Dashboard_View,

            Appointments_View, Appointments_Create, Appointments_Edit, Appointments_Delete,
            Pets_View, Pets_Create, Pets_Edit, Pets_Delete,
            Owners_View, Owners_Create, Owners_Edit, Owners_Delete,
            Inventory_View, Inventory_Create, Inventory_Edit, Inventory_Delete,
            Reports_View,
            Settings_View, Settings_Edit,
            Roles_View, Roles_Edit
        };
    }
}
