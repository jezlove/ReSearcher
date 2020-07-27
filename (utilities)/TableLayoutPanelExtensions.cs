/*
	C# "TableLayoutPanelExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;

namespace ReSearcher {

	public static class TableLayoutPanelExtensions {

		#region column-styles

			public static TTableLayoutPanel withColumnStyles<TTableLayoutPanel>(this TTableLayoutPanel thisTableLayoutPanel, params ColumnStyle[] columnStyles) where TTableLayoutPanel : TableLayoutPanel {
				foreach(ColumnStyle columnStyle in columnStyles) {
					thisTableLayoutPanel.ColumnStyles.Add(columnStyle);
				}
				return(thisTableLayoutPanel);
			}

		#endregion

		#region column-spans

			public static TTableLayoutPanel withColumnSpan<TTableLayoutPanel>(this TTableLayoutPanel thisTableLayoutPanel, Control control, int columnSpan) where TTableLayoutPanel : TableLayoutPanel {
				thisTableLayoutPanel.SetColumnSpan(control, columnSpan);
				return(thisTableLayoutPanel);
			}

		#endregion

		#region row-spans

			public static TTableLayoutPanel withRowSpan<TTableLayoutPanel>(this TTableLayoutPanel thisTableLayoutPanel, Control control, int rowSpan) where TTableLayoutPanel : TableLayoutPanel {
				thisTableLayoutPanel.SetRowSpan(control, rowSpan);
				return(thisTableLayoutPanel);
			}

		#endregion

		#region cell-positions

			public static TTableLayoutPanel withCellPosition<TTableLayoutPanel>(this TTableLayoutPanel thisTableLayoutPanel, Control control, TableLayoutPanelCellPosition tableLayoutPanelCellPosition) where TTableLayoutPanel : TableLayoutPanel {
				thisTableLayoutPanel.SetCellPosition(control, tableLayoutPanelCellPosition);
				return(thisTableLayoutPanel);
			}

			public static TTableLayoutPanel withCellPosition<TTableLayoutPanel>(this TTableLayoutPanel thisTableLayoutPanel, Control control, int column, int row) where TTableLayoutPanel : TableLayoutPanel {
				return(thisTableLayoutPanel.withCellPosition(control, new TableLayoutPanelCellPosition(column, row)));
			}

		#endregion

	}

}