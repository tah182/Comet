using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    public class ExcelData {
        private int _pages;
        private int _displayPage = 1;
        private DataTable _dataTable;
        private bool _fitsTemplate;
        private ErrorType? _errorOut;
        
        private string[] validColumns = {"reference_id", 
                                        "bldg_clli", 
                                        "swc_clli", 
                                        "npa", 
                                        "nxx", 
                                        "address", 
                                        "city", 
                                        "state_code", 
                                        "postal_code", 
                                        "product", 
                                        "service_type", 
                                        "transaction_type", 
                                        "NNI_vendor", 
                                        "service_bandwidth/CP_port_speed", 
                                        "form_factor", 
                                        "EVC", 
                                        "service_term", 
                                        "class_of_service", 
                                        "solution_type", 
                                        "include_avoid_single_vendor", 
                                        "max_sol", 
                                        "country_code", 
                                        "protection", 
                                        "co_termination", 
                                        "avoid_override_term", 
                                        "include_unapproved_ilec", 
                                        "max_asr", 
                                        "avoid_typeII",
                                        "GoogleLat", 
                                        "GoogleLng"};

        
        public ExcelData(DataTable dataTable) {
            this._dataTable = dataTable;
            _errorOut = null;
        }

        public ExcelData(ErrorType errorType) {
            this._errorOut = errorType;
        }

        public ExcelData() {
            _errorOut = null;
        }

        public DataTable dataTable {
            get { return this._dataTable; }
            set {
                this._dataTable = value;
                foreach (DataColumn col in _dataTable.Columns) {
                    if (Array.IndexOf(validColumns, col.ColumnName) <= -1) {
                        _fitsTemplate = false;
                        return;
                    } else
                        _fitsTemplate = true;
                }
            }
        }

        public void setDataTable(System.IO.Stream fileStream) {
            using (var excel = new ExcelPackage(fileStream)) {
                    DataTable dt = new DataTable();
                    var workSheet = excel.Workbook.Worksheets.First();
                    bool hasHeader = true;
                    foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                        dt.Columns.Add(hasHeader ? firstRowCell.Text : String.Format("Column {0}", firstRowCell.Start.Column));
                        //dt.Columns.Add(workSheet.Row(0).Cell(i).Value.ToString(), getTypeFromExcel(workSheet.Row(1).Cell(i).DataType.ToString()));

                    int startRow = hasHeader ? 2 : 1;
                    List<int> referenceID = new List<int>();
                    for (int rowNum = startRow; rowNum <= workSheet.Dimension.End.Row; rowNum++) {
                        var wsRow = workSheet.Cells[rowNum, 1, rowNum, workSheet.Dimension.End.Column];
                        var referenceCell = workSheet.Cells[rowNum, 1, rowNum, workSheet.Dimension.Start.Column];
                        
                        DataRow row = dt.NewRow();
                        int col = 0;
                        foreach (var cell in wsRow) {
                            if (!referenceID.Any() && col == 0) { // check against existing reference_id's
                                if (cell.Text.Trim().Length > 0)
                                    if (referenceID.Contains(int.Parse(cell.Text)))
                                        throw new Exception("Duplicate reference_id");
                            } else if (col == 0)
                                if (cell.Text.Trim().Length > 0)
                                    referenceID.Add(int.Parse(cell.Text));
                            
                            row[cell.Start.Column - 1] = cell.Text;
                            col++;
                        }
                        if (row[0].ToString().Trim().Length > 0)
                            dt.Rows.Add(row);
                    }
                    if (dt.Rows.Count > 0)
                        this._dataTable = dt;
            }
            
        }
        
        public int displayPage {
            get { return this._displayPage; }
            set { this._displayPage = value; }
        }

        public int pages {
            get { return this._pages; }
            set { this._pages = value; }
        }

        public bool fitsTemplate {
            get { return this._fitsTemplate; }
        }

        public ErrorType? errorType {
            set { this._errorOut = value; }
            get { return this._errorOut; }
        }

        private Type getTypeFromExcel(string type) {
            switch (type) {
                case "Text":
                    return Type.GetType("System.String");
                case "DateTime":
                    return Type.GetType("System.DateTime");
                case "Numeric":
                    return Type.GetType("System.Long");
                case "Boolean":
                    return Type.GetType("System.Boolean");
                default:
                    return null;
            }
        }
    }

    public enum ErrorType {
        ReferenceIdDup, ExcelFormat, GoogleAPI, FileExtension, SessionOutdated, HistoricalMaxConnection, ImportStoredProcedure, ExcelOutput
    }
}