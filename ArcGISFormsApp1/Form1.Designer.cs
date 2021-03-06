namespace ArcGISFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ImportMXD = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打开表格ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.缩放至图层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnImportMDB = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnConnServer = new System.Windows.Forms.Button();
            this.btnPublish = new System.Windows.Forms.Button();
            this.btnShowLayout = new System.Windows.Forms.Button();
            this.btnImportIMG = new System.Windows.Forms.Button();
            this.btnSelectFeature = new System.Windows.Forms.Button();
            this.btnDelAllTable = new System.Windows.Forms.Button();
            this.btnAddDataset = new System.Windows.Forms.Button();
            this.btnDelTable = new System.Windows.Forms.Button();
            this.btnAddTable = new System.Windows.Forms.Button();
            this.btnClearPolygon_Custom = new System.Windows.Forms.Button();
            this.btnCacheQuery = new System.Windows.Forms.Button();
            this.btnAddPolygon_Custom = new System.Windows.Forms.Button();
            this.btnClearLayer = new System.Windows.Forms.Button();
            this.btnClearSelectPolygon = new System.Windows.Forms.Button();
            this.btnSelectPolygon = new System.Windows.Forms.Button();
            this.btnAddPolygon = new System.Windows.Forms.Button();
            this.btnAddPolyline = new System.Windows.Forms.Button();
            this.btnAddPoint = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.datalistBox = new System.Windows.Forms.ListBox();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.axPageLayoutControl1 = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImportMXD
            // 
            this.ImportMXD.Location = new System.Drawing.Point(3, 12);
            this.ImportMXD.Name = "ImportMXD";
            this.ImportMXD.Size = new System.Drawing.Size(170, 23);
            this.ImportMXD.TabIndex = 4;
            this.ImportMXD.Text = "导入MXD";
            this.ImportMXD.UseVisualStyleBackColor = true;
            this.ImportMXD.Click += new System.EventHandler(this.ImportMXD_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开表格ToolStripMenuItem,
            this.缩放至图层ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 48);
            // 
            // 打开表格ToolStripMenuItem
            // 
            this.打开表格ToolStripMenuItem.Name = "打开表格ToolStripMenuItem";
            this.打开表格ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.打开表格ToolStripMenuItem.Text = "打开表格";
            this.打开表格ToolStripMenuItem.Click += new System.EventHandler(this.打开表格ToolStripMenuItem_Click);
            // 
            // 缩放至图层ToolStripMenuItem
            // 
            this.缩放至图层ToolStripMenuItem.Name = "缩放至图层ToolStripMenuItem";
            this.缩放至图层ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.缩放至图层ToolStripMenuItem.Text = "缩放至图层";
            this.缩放至图层ToolStripMenuItem.Click += new System.EventHandler(this.缩放至图层ToolStripMenuItem_Click);
            // 
            // btnImportMDB
            // 
            this.btnImportMDB.Location = new System.Drawing.Point(3, 41);
            this.btnImportMDB.Name = "btnImportMDB";
            this.btnImportMDB.Size = new System.Drawing.Size(170, 23);
            this.btnImportMDB.TabIndex = 6;
            this.btnImportMDB.Text = "导入Mdb全部图层";
            this.btnImportMDB.UseVisualStyleBackColor = true;
            this.btnImportMDB.Click += new System.EventHandler(this.btnImportMDB_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnConnServer);
            this.panel1.Controls.Add(this.btnPublish);
            this.panel1.Controls.Add(this.btnShowLayout);
            this.panel1.Controls.Add(this.btnImportIMG);
            this.panel1.Controls.Add(this.btnSelectFeature);
            this.panel1.Controls.Add(this.btnDelAllTable);
            this.panel1.Controls.Add(this.btnAddDataset);
            this.panel1.Controls.Add(this.btnDelTable);
            this.panel1.Controls.Add(this.btnAddTable);
            this.panel1.Controls.Add(this.btnClearPolygon_Custom);
            this.panel1.Controls.Add(this.btnCacheQuery);
            this.panel1.Controls.Add(this.btnAddPolygon_Custom);
            this.panel1.Controls.Add(this.btnClearLayer);
            this.panel1.Controls.Add(this.btnClearSelectPolygon);
            this.panel1.Controls.Add(this.btnSelectPolygon);
            this.panel1.Controls.Add(this.btnAddPolygon);
            this.panel1.Controls.Add(this.btnAddPolyline);
            this.panel1.Controls.Add(this.btnAddPoint);
            this.panel1.Controls.Add(this.ImportMXD);
            this.panel1.Controls.Add(this.btnImportMDB);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(179, 715);
            this.panel1.TabIndex = 9;
            // 
            // btnConnServer
            // 
            this.btnConnServer.Location = new System.Drawing.Point(3, 562);
            this.btnConnServer.Name = "btnConnServer";
            this.btnConnServer.Size = new System.Drawing.Size(170, 23);
            this.btnConnServer.TabIndex = 24;
            this.btnConnServer.Text = "连接ArcgisServer";
            this.btnConnServer.UseVisualStyleBackColor = true;
            this.btnConnServer.Click += new System.EventHandler(this.btnConnServer_Click);
            // 
            // btnPublish
            // 
            this.btnPublish.Location = new System.Drawing.Point(3, 533);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(170, 23);
            this.btnPublish.TabIndex = 23;
            this.btnPublish.Text = "发布服务";
            this.btnPublish.UseVisualStyleBackColor = true;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // btnShowLayout
            // 
            this.btnShowLayout.Location = new System.Drawing.Point(3, 504);
            this.btnShowLayout.Name = "btnShowLayout";
            this.btnShowLayout.Size = new System.Drawing.Size(170, 23);
            this.btnShowLayout.TabIndex = 22;
            this.btnShowLayout.Text = "显示布局视图";
            this.btnShowLayout.UseVisualStyleBackColor = true;
            this.btnShowLayout.Click += new System.EventHandler(this.btnShowLayout_Click);
            // 
            // btnImportIMG
            // 
            this.btnImportIMG.Location = new System.Drawing.Point(3, 475);
            this.btnImportIMG.Name = "btnImportIMG";
            this.btnImportIMG.Size = new System.Drawing.Size(170, 23);
            this.btnImportIMG.TabIndex = 21;
            this.btnImportIMG.Text = "导出图片";
            this.btnImportIMG.UseVisualStyleBackColor = true;
            this.btnImportIMG.Click += new System.EventHandler(this.btnImportIMG_Click);
            // 
            // btnSelectFeature
            // 
            this.btnSelectFeature.Location = new System.Drawing.Point(3, 446);
            this.btnSelectFeature.Name = "btnSelectFeature";
            this.btnSelectFeature.Size = new System.Drawing.Size(170, 23);
            this.btnSelectFeature.TabIndex = 20;
            this.btnSelectFeature.Text = "选择要素工具";
            this.btnSelectFeature.UseVisualStyleBackColor = true;
            this.btnSelectFeature.Click += new System.EventHandler(this.btnSelectFeature_Click);
            // 
            // btnDelAllTable
            // 
            this.btnDelAllTable.Location = new System.Drawing.Point(3, 388);
            this.btnDelAllTable.Name = "btnDelAllTable";
            this.btnDelAllTable.Size = new System.Drawing.Size(170, 23);
            this.btnDelAllTable.TabIndex = 19;
            this.btnDelAllTable.Text = "删除全部类（表）";
            this.btnDelAllTable.UseVisualStyleBackColor = true;
            this.btnDelAllTable.Click += new System.EventHandler(this.btnDelAllTable_Click);
            // 
            // btnAddDataset
            // 
            this.btnAddDataset.Location = new System.Drawing.Point(3, 417);
            this.btnAddDataset.Name = "btnAddDataset";
            this.btnAddDataset.Size = new System.Drawing.Size(170, 23);
            this.btnAddDataset.TabIndex = 18;
            this.btnAddDataset.Text = "新建要素集";
            this.btnAddDataset.UseVisualStyleBackColor = true;
            this.btnAddDataset.Click += new System.EventHandler(this.btnAddDataset_Click);
            // 
            // btnDelTable
            // 
            this.btnDelTable.Location = new System.Drawing.Point(3, 359);
            this.btnDelTable.Name = "btnDelTable";
            this.btnDelTable.Size = new System.Drawing.Size(170, 23);
            this.btnDelTable.TabIndex = 17;
            this.btnDelTable.Text = "删除指定名称的类（表）";
            this.btnDelTable.UseVisualStyleBackColor = true;
            this.btnDelTable.Click += new System.EventHandler(this.btnDelTable_Click);
            // 
            // btnAddTable
            // 
            this.btnAddTable.Location = new System.Drawing.Point(3, 330);
            this.btnAddTable.Name = "btnAddTable";
            this.btnAddTable.Size = new System.Drawing.Size(170, 23);
            this.btnAddTable.TabIndex = 16;
            this.btnAddTable.Text = "新建要素类";
            this.btnAddTable.UseVisualStyleBackColor = true;
            this.btnAddTable.Click += new System.EventHandler(this.btnAddTable_Click);
            // 
            // btnClearPolygon_Custom
            // 
            this.btnClearPolygon_Custom.Location = new System.Drawing.Point(3, 214);
            this.btnClearPolygon_Custom.Name = "btnClearPolygon_Custom";
            this.btnClearPolygon_Custom.Size = new System.Drawing.Size(170, 23);
            this.btnClearPolygon_Custom.TabIndex = 15;
            this.btnClearPolygon_Custom.Text = "清空【面】图像元素";
            this.btnClearPolygon_Custom.UseVisualStyleBackColor = true;
            this.btnClearPolygon_Custom.Click += new System.EventHandler(this.btnClearPolygon_Custom_Click);
            // 
            // btnCacheQuery
            // 
            this.btnCacheQuery.Location = new System.Drawing.Point(3, 301);
            this.btnCacheQuery.Name = "btnCacheQuery";
            this.btnCacheQuery.Size = new System.Drawing.Size(170, 23);
            this.btnCacheQuery.TabIndex = 14;
            this.btnCacheQuery.Text = "缓存查询（需先点击地图）";
            this.btnCacheQuery.UseVisualStyleBackColor = true;
            this.btnCacheQuery.Click += new System.EventHandler(this.btnCacheQuery_Click);
            // 
            // btnAddPolygon_Custom
            // 
            this.btnAddPolygon_Custom.Location = new System.Drawing.Point(3, 185);
            this.btnAddPolygon_Custom.Name = "btnAddPolygon_Custom";
            this.btnAddPolygon_Custom.Size = new System.Drawing.Size(170, 23);
            this.btnAddPolygon_Custom.TabIndex = 13;
            this.btnAddPolygon_Custom.Text = "添加【面】图像元素";
            this.btnAddPolygon_Custom.UseVisualStyleBackColor = true;
            this.btnAddPolygon_Custom.Click += new System.EventHandler(this.btnAddPolygon_Custom_Click);
            // 
            // btnClearLayer
            // 
            this.btnClearLayer.Location = new System.Drawing.Point(3, 70);
            this.btnClearLayer.Name = "btnClearLayer";
            this.btnClearLayer.Size = new System.Drawing.Size(170, 23);
            this.btnClearLayer.TabIndex = 12;
            this.btnClearLayer.Text = "清除全部图层";
            this.btnClearLayer.UseVisualStyleBackColor = true;
            this.btnClearLayer.Click += new System.EventHandler(this.btnClearLayer_Click);
            // 
            // btnClearSelectPolygon
            // 
            this.btnClearSelectPolygon.Location = new System.Drawing.Point(3, 272);
            this.btnClearSelectPolygon.Name = "btnClearSelectPolygon";
            this.btnClearSelectPolygon.Size = new System.Drawing.Size(170, 23);
            this.btnClearSelectPolygon.TabIndex = 11;
            this.btnClearSelectPolygon.Text = "清空高亮选择";
            this.btnClearSelectPolygon.UseVisualStyleBackColor = true;
            this.btnClearSelectPolygon.Click += new System.EventHandler(this.btnClearSelectPolygon_Click);
            // 
            // btnSelectPolygon
            // 
            this.btnSelectPolygon.Location = new System.Drawing.Point(3, 243);
            this.btnSelectPolygon.Name = "btnSelectPolygon";
            this.btnSelectPolygon.Size = new System.Drawing.Size(170, 23);
            this.btnSelectPolygon.TabIndex = 10;
            this.btnSelectPolygon.Text = "高亮选择【面】的交叉图形";
            this.btnSelectPolygon.UseVisualStyleBackColor = true;
            this.btnSelectPolygon.Click += new System.EventHandler(this.btnSelectPolygon_Click);
            // 
            // btnAddPolygon
            // 
            this.btnAddPolygon.Location = new System.Drawing.Point(3, 156);
            this.btnAddPolygon.Name = "btnAddPolygon";
            this.btnAddPolygon.Size = new System.Drawing.Size(170, 23);
            this.btnAddPolygon.TabIndex = 9;
            this.btnAddPolygon.Text = "画【面】图形";
            this.btnAddPolygon.UseVisualStyleBackColor = true;
            this.btnAddPolygon.Click += new System.EventHandler(this.btnAddPolygon_Click);
            // 
            // btnAddPolyline
            // 
            this.btnAddPolyline.Location = new System.Drawing.Point(3, 127);
            this.btnAddPolyline.Name = "btnAddPolyline";
            this.btnAddPolyline.Size = new System.Drawing.Size(170, 23);
            this.btnAddPolyline.TabIndex = 8;
            this.btnAddPolyline.Text = "画【线】图形";
            this.btnAddPolyline.UseVisualStyleBackColor = true;
            this.btnAddPolyline.Click += new System.EventHandler(this.btnAddPolyline_Click);
            // 
            // btnAddPoint
            // 
            this.btnAddPoint.Location = new System.Drawing.Point(3, 98);
            this.btnAddPoint.Name = "btnAddPoint";
            this.btnAddPoint.Size = new System.Drawing.Size(170, 23);
            this.btnAddPoint.TabIndex = 7;
            this.btnAddPoint.Text = "画【点】图形";
            this.btnAddPoint.UseVisualStyleBackColor = true;
            this.btnAddPoint.Click += new System.EventHandler(this.btnAddPoint_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.axToolbarControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(179, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1205, 35);
            this.panel2.TabIndex = 10;
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 0);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(1205, 28);
            this.axToolbarControl1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.datalistBox);
            this.panel3.Controls.Add(this.axTOCControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(179, 35);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(239, 680);
            this.panel3.TabIndex = 11;
            // 
            // datalistBox
            // 
            this.datalistBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datalistBox.FormattingEnabled = true;
            this.datalistBox.ItemHeight = 12;
            this.datalistBox.Location = new System.Drawing.Point(0, 302);
            this.datalistBox.Name = "datalistBox";
            this.datalistBox.Size = new System.Drawing.Size(239, 378);
            this.datalistBox.TabIndex = 7;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axTOCControl1.Location = new System.Drawing.Point(0, 0);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(239, 302);
            this.axTOCControl1.TabIndex = 0;
            this.axTOCControl1.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.axTOCControl1_OnMouseDown);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.axPageLayoutControl1);
            this.panel4.Controls.Add(this.axMapControl1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(418, 35);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(966, 658);
            this.panel4.TabIndex = 12;
            // 
            // axPageLayoutControl1
            // 
            this.axPageLayoutControl1.Location = new System.Drawing.Point(471, 6);
            this.axPageLayoutControl1.Name = "axPageLayoutControl1";
            this.axPageLayoutControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPageLayoutControl1.OcxState")));
            this.axPageLayoutControl1.Size = new System.Drawing.Size(476, 649);
            this.axPageLayoutControl1.TabIndex = 1;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(6, 6);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(459, 649);
            this.axMapControl1.TabIndex = 0;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl1_OnMouseMove);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(418, 693);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(966, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 715);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ImportMXD;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 打开表格ToolStripMenuItem;
        private System.Windows.Forms.Button btnImportMDB;
        private System.Windows.Forms.ListBox datalistBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private System.Windows.Forms.Panel panel3;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private System.Windows.Forms.Panel panel4;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnAddPolygon;
        private System.Windows.Forms.Button btnAddPolyline;
        private System.Windows.Forms.Button btnAddPoint;
        private System.Windows.Forms.Button btnSelectPolygon;
        private System.Windows.Forms.Button btnClearSelectPolygon;
        private System.Windows.Forms.Button btnClearLayer;
        private System.Windows.Forms.Button btnAddPolygon_Custom;
        private System.Windows.Forms.Button btnCacheQuery;
        private System.Windows.Forms.Button btnClearPolygon_Custom;
        private System.Windows.Forms.Button btnAddTable;
        private System.Windows.Forms.Button btnDelTable;
        private System.Windows.Forms.Button btnAddDataset;
        private System.Windows.Forms.Button btnDelAllTable;
        private System.Windows.Forms.Button btnSelectFeature;
        private System.Windows.Forms.Button btnImportIMG;
        private ESRI.ArcGIS.Controls.AxPageLayoutControl axPageLayoutControl1;
        private System.Windows.Forms.Button btnShowLayout;
        private System.Windows.Forms.ToolStripMenuItem 缩放至图层ToolStripMenuItem;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.Button btnConnServer;
    }
}

