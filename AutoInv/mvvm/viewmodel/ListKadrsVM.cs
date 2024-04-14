using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoKadr.mvvm.model;
using AutoKadr.mvvm.view;

namespace AutoKadr.mvvm.viewmodel
{
    public class ListKadrsVM : BaseVM
    {
        private MainVM mainVM;
        private string searchText = "";
        private ObservableCollection<Kadr> kadrs;
        private Post selectedPost;

        public VmCommand Create { get; set; }
        public VmCommand Edit { get; set; }
        public VmCommand Delete { get; set; }

        public Post SelectedPost
        {
            get => selectedPost;
            set
            {
                selectedPost = value;
                Signal();
                Search();
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
            }
        }

        public ObservableCollection<Post> AllPosts { get; set; }
        public Kadr SelectedKadr { get; set; }
        public ObservableCollection<Kadr> Kadrs
        {
            get => kadrs;
            set
            {
                kadrs = value;
                Signal();
            }
        }

        public ListKadrsVM()
        {   
            AllPosts = new ObservableCollection<Post>(PostRepository.Instance.GetPosts());
            AllPosts.Insert(0, new Post { Id = 0, Name = "Все должности" });
            SelectedPost = AllPosts[0];
            string sql = "SELECT k.id, k.name, k.surname, k.otchestvo, p.id AS postId, p.Name AS tagTitle FROM CrossKadrPost ckp, Kadr k, Post p WHERE ckp.id_Kadr = k.id AND ckp.id_Post = p.id";

            Kadrs = new ObservableCollection<Kadr>(KadrRepository.Instance.GetAllKadrs(sql));

            Create = new VmCommand(() =>
            {
                mainVM.CurrentPage = new EditorKadr(mainVM);
            });

            Edit = new VmCommand(() => {
                if (SelectedPost == null)
                    return;
                mainVM.CurrentPage = new EditorKadr(mainVM, SelectedKadr);
            });

            Delete = new VmCommand(() => {
                if (SelectedKadr == null)
                    return;

                if (MessageBox.Show("Удаление сотрудника", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    KadrRepository.Instance.Remove(SelectedKadr);
                    Kadrs.Remove(SelectedKadr);
                }
            });

        }

        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }

        private void Search()
        {
            Kadrs = new ObservableCollection<Kadr>(
                KadrRepository.Instance.Search(SearchText, SelectedPost));

        }
    }
}
