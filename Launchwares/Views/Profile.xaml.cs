using Launchwares.Core;
using Launchwares.CustomElements;
using LaunchwaresCore;
using LaunchwaresCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Launchwares.Views
{
    /// <summary>
    /// Profile.xaml etkileşim mantığı
    /// </summary>
    public partial class Profile : Page
    {
        public Profile()
        {
            InitializeComponent();
        }

        public Profile(ulong User)
        {
            InitializeComponent();

            this.User = Utils.Users.Where(x => x.Uid == User).FirstOrDefault();
            this.Username.Text = Utils.GetUsername(User);
            this.UserPhoto.ImageSource = Utils.GetProfilePhoto(User);
            this.Badge.Kind = Utils.GetBadge((Models.UserType)this.User.Usertype);
            GetPosts();
        }

        async void GetPosts()
        {
            var posts = await API.client.Get<List<Models.Post>>($"posts/{API.client.Token.slug}");
            posts = posts.Where(x => x.Author == User.Uid).ToList();

            int i = 0;
            foreach (var post in posts) {
                if (i % 2 == 0)
                    PostContainerRight.Children.Add(new PostBox(post));
                else
                    PostContainerLeft.Children.Add(new PostBox(post));
                i++;
            }
        }

        public Models.User User { get; set; }
    }
}
