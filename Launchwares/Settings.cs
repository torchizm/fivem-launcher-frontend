namespace Launchwares.Properties
{


    // Bu sınıf ayarlar sınıfında belirli olayları işleyebilmenizi sağlar:
    //  SettingChanging olayı, bir ayarın değeri değiştirilmeden önce tetiklenir.
    //  PropertyChanged olayı, bir ayarın değeri değiştirildikten sonra tetiklenir.
    //  SettingsLoaded olayı ayar değerleri yüklendikten sonra oluşturulur.
    //  SettingsSaving olayı, ayar değerleri kaydedilmeden önce tetiklenir.
    internal sealed partial class Settings
    {

        public Settings()
        {
            // // Ayarları kaydetmek ve değiştirmek üzere olay işleyicileri eklemek için alttaki açıklama satırlarını kaldırın:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }

        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            // SettingChangingEvent olayını işleyebilmek için buraya kod ekleyin.
        }

        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // SettingsSaving olayını işleyebilmek için buraya kod ekleyin.
        }
    }
}
