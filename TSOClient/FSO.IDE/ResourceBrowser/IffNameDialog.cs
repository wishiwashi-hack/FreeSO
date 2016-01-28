﻿using FSO.Files.Formats.IFF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSO.IDE.ResourceBrowser
{
    public partial class IffNameDialog : Form
    {
        private IffChunk Chunk;
        private bool NewChunk;

        public IffNameDialog(IffChunk chunk, bool newChunk)
        {
            InitializeComponent();

            Chunk = chunk;
            NewChunk = newChunk;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            var iff = Chunk.ChunkParent;

            MethodInfo method = typeof(IffFile).GetMethod("Get");
            MethodInfo generic = method.MakeGenericMethod(Chunk.GetType());
            var chunk = (object[])generic.Invoke(iff, new object[] { (short)ChunkIDEntry.Value });

            if (chunk != null)
            {
                MessageBox.Show("The specified ID is already in use!", "Yikes!");
            }
            else
            {
                Chunk.ChunkID = (ushort)ChunkIDEntry.Value;

                if (!NewChunk) iff.RemoveChunk(Chunk);
                iff.AddChunk(Chunk);

                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
