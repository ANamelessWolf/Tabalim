using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Addin.Model;
using static Tabalim.Addin.Assets.AddinConstants;
namespace Tabalim.Addin.Controller
{
    public class FormatSaver
    {
        public const String TABALIM_DIC = "Tabalim";
        public const String TAG_HOR_REC = "HorizontalTags";
        public const String TAG_VER_REC = "VerticalTags";
        /// <summary>
        /// The alimentador rows
        /// </summary>
        public AlimentadorRow[] Rows;
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatSaver"/> class.
        /// </summary>
        /// <param name="rows">The rows.</param>
        public FormatSaver(AlimentadorRow[] rows)
        {
            this.Rows = rows;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatSaver"/> class.
        /// </summary>
        /// <param name="rows">The rows.</param>
        public FormatSaver()
        {
            this.Rows = new AlimentadorRow[0];
        }
        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="tr">The tr.</param>
        public void Save(Document doc, Transaction tr)
        {
            DBDictionary NOD = (DBDictionary)doc.Database.NamedObjectsDictionaryId.GetObject(OpenMode.ForRead),
                tabalimDic;
            ObjectId dicId, horId;
            //Se abre el diccionario
            if (this.TryGetEntry(NOD, TABALIM_DIC, out dicId))
                tabalimDic = (DBDictionary)dicId.GetObject(OpenMode.ForRead);
            else
            {
                DBDictionary newDictionary = new DBDictionary();
                NOD.UpgradeOpen();
                NOD.SetAt(TABALIM_DIC, newDictionary);
                tr.AddNewlyCreatedDBObject(newDictionary, true);
                tabalimDic = newDictionary;
            }
            //Se abre el registro
            this.SaveRecord(tr, tabalimDic, TAG_HOR_REC, this.Rows.Select(x => x.GetHorizontalFormat()));
            this.SaveRecord(tr, tabalimDic, TAG_VER_REC, this.Rows.Select(x => x.GetVerticalFormat()));
        }
        /// <summary>
        /// Guarda el registro de las filas dibujadas
        /// </summary>
        /// <param name="tr">La transacción activa.</param>
        /// <param name="tabalimDic">El diccionario Tabalim.</param>
        /// <param name="entryName">El nombre de la entrada</param>
        /// <param name="data">La información a guardar en el registro.</param>
        private void SaveRecord(Transaction tr, DBDictionary tabalimDic, string entryName, IEnumerable<string> data)
        {
            if (data.Count() > 0)
            {
                ObjectId entryId;
                Xrecord xRec;
                if (this.TryGetEntry(tabalimDic, entryName, out entryId))
                    xRec = (Xrecord)entryId.GetObject(OpenMode.ForRead);
                else
                {
                    tabalimDic.UpgradeOpen();
                    Xrecord newXrecord = new Xrecord();
                    tabalimDic.SetAt(entryName, newXrecord);
                    tr.AddNewlyCreatedDBObject(newXrecord, true);
                    xRec = newXrecord;
                }
                xRec.UpgradeOpen();
                TypedValue[] tpVal = data.Select(x => new TypedValue((int)DxfCode.Text, x)).ToArray();
                xRec.Data = new ResultBuffer(tpVal);
            }
        }
        /// <summary>
        /// Inserta las etiquetas horizontales
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="tr">The tr.</param>
        public void InsertHorizontalTags(Document doc, Transaction tr)
        {
            this.InsertTags(doc, tr, TAG_HOR_REC);
        }
        /// <summary>
        /// Inserta las etiquetas verticales
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="tr">The tr.</param>
        public void InsertVerticalTags(Document doc, Transaction tr)
        {
            this.InsertTags(doc, tr, TAG_VER_REC);
        }
        /// <summary>
        /// Inserts the tags.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="tr">The tr.</param>
        /// <param name="entryName">Name of the entry.</param>
        private void InsertTags(Document doc, Transaction tr, string entryName)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            DBDictionary NOD = (DBDictionary)doc.Database.NamedObjectsDictionaryId.GetObject(OpenMode.ForRead),
            tabalimDic;
            Xrecord rec;
            ObjectId dicId, recordId;
            if (this.TryGetEntry(NOD, TABALIM_DIC, out dicId))
            {
                tabalimDic = (DBDictionary)dicId.GetObject(OpenMode.ForRead);
                if (this.TryGetEntry(tabalimDic, entryName, out recordId))
                {
                    rec = (Xrecord)recordId.GetObject(OpenMode.ForRead);
                    var res = ed.GetPoint("Selecciona el punto de inserción de las etiquetas");
                    if (res.Status == PromptStatus.OK && rec.Data != null)
                    {
                        var data = rec.Data.OfType<TypedValue>().Select(x => x.Value.ToString());
                        Point3d insPoint = res.Value;
                        MText text, prevText = null;
                        Double startX = insPoint.X;
                        BlockTableRecord model = (BlockTableRecord)doc.Database.CurrentSpaceId.GetObject(OpenMode.ForWrite);
                        Vector3d offset = entryName == TAG_HOR_REC ? new Vector3d(0, TEXTHEIGHT * -4, 0) : new Vector3d(0, TEXTHEIGHT * -4, 0);

                        foreach (String val in data)
                        {
                            String[] tags = val.Split('@');
                            for (int i = 0; i < tags.Length; i++)
                            {
                                text = new MText();
                                text.SetDatabaseDefaults();
                                text.Contents = tags[i];
                                text.Height = TEXTHEIGHT;
                                text.Rotation = 0;
                                this.UpdateOffset(i, ref insPoint, prevText, text, entryName == TAG_HOR_REC);
                                text.Location = insPoint;
                                model.AppendEntity(text);
                                tr.AddNewlyCreatedDBObject(text, true);
                                //Dos textos en línea horizontal
                                if (entryName == TAG_HOR_REC && i % 2 != 0)
                                    insPoint = new Point3d(startX, insPoint.Y, insPoint.Z);
                                prevText = text;
                            }
                            insPoint = new Point3d(startX, insPoint.Y, insPoint.Z);
                            insPoint += offset;
                        }
                    }
                }
            }
            else
                Application.ShowAlertDialog("Primero inserte una tabla de Alimentadores.");
        }

        private void UpdateOffset(int index, ref Point3d location, MText prvText, MText text, bool isHorizontalAligment)
        {
            Double tagWidth = Math.Abs(text.GeometricExtents.MinPoint.X - text.GeometricExtents.MaxPoint.X),
                   tagPrevWidth = prvText != null ? Math.Abs(prvText.GeometricExtents.MinPoint.X - prvText.GeometricExtents.MaxPoint.X) : 0;
            text.Location = location;
            Vector3d[] offset;
            if (isHorizontalAligment)
                offset = new Vector3d[]
                {
                    new Vector3d(0, 0, 0), //In
                    new Vector3d(tagPrevWidth + TEXTHEIGHT * 1.5, 0, 0), //Cables
                    new Vector3d(0, TEXTHEIGHT * -1.5d, 0), //Conector
                    new Vector3d(tagPrevWidth + TEXTHEIGHT * 1.5, 0, 0), //Longitud
                    new Vector3d(0, TEXTHEIGHT * -1.5d, 0), //e%
                };
            else
                offset = new Vector3d[]
                {
                    new Vector3d(0, 0, 0), //In
                    new Vector3d(0, TEXTHEIGHT*-1.5d, 0), //Cables
                    new Vector3d(0, TEXTHEIGHT*-1.5d, 0), //Conector
                    new Vector3d(0, TEXTHEIGHT*-1.5d, 0), //Longitud
                    new Vector3d(0, TEXTHEIGHT*-1.5d, 0), //e%
                };
            location += offset[index];
        }

        /// <summary>
        /// Tries to open a dictionary entry
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="entryName">Name of the entry.</param>
        /// <param name="entryId">The entry identifier.</param>
        /// <returns>True if the entry exists</returns>
        private bool TryGetEntry(DBDictionary dictionary, string entryName, out ObjectId entryId)
        {
            entryId = new ObjectId();
            try
            {
                entryId = dictionary.GetAt(entryName);
            }
            catch (Exception) { }
            return entryId.IsValid;
        }
    }
}
