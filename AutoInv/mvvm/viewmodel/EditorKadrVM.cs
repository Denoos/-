using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AutoKadr.mvvm.model;
using AutoKadr.mvvm.view;

namespace AutoKadr.mvvm.viewmodel
{
    public class EditorKadrVM : BaseVM
    {

        MainVM mainVM;
        ListBox listPosts;
        private Kadr kadr = new();

        public Kadr Kadr
        {
            get => kadr;
            set
            {
                kadr = value;
                Signal();
            }
        }
        public VmCommand Save { get; set; }
        public List<Post> AllPosts { get; set; }

        public EditorKadrVM()
        {
            AllPosts = PostRepository.Instance.GetPosts();

            Save = new VmCommand(() => {
                Kadr.Posts.Clear();
                foreach (Post post in listPosts.SelectedItems)
                    Kadr.Posts.Add(post);

                if (Kadr.Id == 0)
                    KadrRepository.Instance.AddKadr(Kadr);
                else
                    KadrRepository.Instance.UpdateKadr(Kadr);

                mainVM.CurrentPage = new ListKadrs(mainVM);
            });
        }

        internal void SetMainVM(MainVM mainVM,
            ListBox listPosts)
        {
            this.mainVM = mainVM;
            this.listPosts = listPosts;
        }

        internal void SetEditKadr(Kadr selectedKadr)
        {
            Kadr = selectedKadr;
            foreach (var post in Kadr.Posts)
                listPosts.SelectedItems.Add(post);
        }
    }
}
