using BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DAL.Entitis;
using System.Collections;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.X509Certificates;
using System.Linq.Expressions;

namespace GUI
{
    public partial class frmSanPham : Form
    {
        private readonly ProductService productService = new ProductService();
        public readonly TypeService typeService = new TypeService();

        public frmSanPham()
        {
            InitializeComponent();
        }

        private void lvSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSanPham.SelectedItems.Count > 0)
            {
                ListViewItem item = lvSanPham.SelectedItems[0];
                txtidSP.Text = item.SubItems[0].Text;
                txtnameSP.Text = item.SubItems[1].Text;
                dtNgaynhap.Text = item.SubItems[2].Text;
                cmbLoaiSP.Text = item.SubItems[3].Text;

            }
        }

        public void frmSanPham_Load(object sender, EventArgs e)
        {
            var listType = TypeService.GetAll();
            var listProduct = ProductService.GetAll();
            FillTypeCombobox(listType);
            BindGrid(listProduct);
        }


        public void FillTypeCombobox(List<Loai> listType)
        {
            listType.Insert(0, new Loai());
            this.cmbLoaiSP.DataSource = listType;
            this.cmbLoaiSP.DisplayMember = "TenLoai";
            this.cmbLoaiSP.ValueMember = "MaLoai";
            ;
        }

      
        public void BindGrid(List<SanPham> listProduct)
        {
            lvSanPham.Items.Clear();
            foreach (var item in listProduct)
            {
                
                ListViewItem listViewItem = new ListViewItem(item.MaSP);
                listViewItem.SubItems.Add(item.TenSP);
                listViewItem.SubItems.Add(item.Ngaynhap.ToString());
                listViewItem.SubItems.Add(item.Loai.TenLoai);

              
                lvSanPham.Items.Add(listViewItem);
            }
        }
        


        
       private void LoadListViewData(List<SanPham> sanphams)
        {
            lvSanPham.Items.Clear();

            foreach (var sanpham in sanphams)
            {
                string[] row = { sanpham.MaSP, sanpham.TenSP, sanpham.Ngaynhap.ToString(), sanpham.Loai.TenLoai.ToString() };
                var listViewItem = new ListViewItem(row);
                lvSanPham.Items.Add(listViewItem);
            }
        } 
       

        
        private void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {
                var listSP = ProductService.GetAll();
                var listLoai = TypeService.GetAll();

                string ma = txtidSP.Text.ToString();
                string ten = txtnameSP.Text.ToString();
                DateTime ngay = dtNgaynhap.Value;
                var loai = listLoai.FirstOrDefault(l => l.TenLoai == cmbLoaiSP.Text);

                var sp = new SanPham
                {
                    MaSP = ma,
                    TenSP = ten,
                    Ngaynhap = ngay,
                    MaLoai = loai.MaLoai
                };
                ProductService.InsertUpdate(sp);
                BindGrid(listSP);

                MessageBox.Show("Thêm sản phẩm thành công");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private int GetSelectedRow(string productID)
        {
            for (int i = 0; i < lvSanPham.Items.Count; i++)
            {
                if (lvSanPham.Items[i].SubItems[0].Text == productID)
                {
                    return i;
                }
            }
            return -1;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedRow = GetSelectedRow(txtidSP.Text);
                if (selectedRow != -1)
                {
                    ProductModel context = new ProductModel();


                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên " + txtidSP.Text, "Xac Nhan", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        var product = context.SanPhams.Find(txtidSP.Text);
                        context.SanPhams.Remove(product);
                        context.SaveChanges();
                        lvSanPham.Items.RemoveAt(selectedRow);
                        MessageBox.Show("Xoa Thanh Cong");

                        txtidSP.Text = "";
                        txtnameSP.Text = "";
                        dtNgaynhap.Text = "";
                        cmbLoaiSP.SelectedIndex = -1;

                    }
                }
            }
            catch
            {
                MessageBox.Show("Xoa that bai!!!!");
            }
        }
        public void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<SanPham> searchResults0 = ProductService.GetAll();          
            string keyword = txtTim.Text.Trim();

            if (!string.IsNullOrEmpty(keyword))
            {
                List<SanPham> searchResults1 = ProductService.SearchSanphamByTenSP(keyword);

                if (searchResults1.Count > 0)
                {
                    BindGrid(searchResults1);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm nào.");
                    BindGrid(searchResults1);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.");
                BindGrid(searchResults0);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvSanPham.SelectedItems.Count > 0)
            {               
                ListViewItem selectedItem = lvSanPham.SelectedItems[0];
                string maSP = selectedItem.SubItems[0].Text;
                string tenSP = txtnameSP.Text;
                string ngayNhap = dtNgaynhap.Text;
                string maLoai = cmbLoaiSP.SelectedValue?.ToString();

                ProductService.UpdateSanpham(maSP, tenSP, ngayNhap, maLoai);
                MessageBox.Show("Sửa sản phẩm thành công.");

                List<SanPham> updatedSearchResults = ProductService.GetAll();
                BindGrid(updatedSearchResults);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để sửa.");
            }
        }
    }
    }

        
      
