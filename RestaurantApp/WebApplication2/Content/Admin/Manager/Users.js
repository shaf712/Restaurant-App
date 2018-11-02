$(document).ready(function () {
	$("#UserGrid").kendoGrid({
		dataSource: {
			transport:
			{
				read: function (options) {
					options.success(list);
				},
				destroy: function (options) {
					console.log(options.data);
					$.ajax
					({
						url: '/Manager/SuspendUser',
						type: "POST",
						data: { 'UserID': options.data.ID },
						success: function (result) {
							options.success();
						}
					});
				}
			},
			schema: {
				model: {
					id: "ID",
					fields:
					{
						Username: { type: "string" },
						Balance: { type: "number" },
						VIP: { type: "number" },
						Verified: { type: "number" },
						WarningCount: {type: "number"}, 
						MoneySpent: { type: "number" },
						DemotionCount: { type: "number" }
					}
				}
			}
		},

		detailInit: detailInit,
		columns:
		[
			{ field: "Username", title: "Name", template: function (dataItem) { return "<center>" + dataItem.Username + "</center>" } },
			{ field: "Balance", title: "Balance", template: function (dataItem) { return "<center>"+ '$' + dataItem.Balance.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "</center>"; } },
			{ field: "VIP", title: "VIP Status", template: function (dataItem) { return dataItem.VIP == 1 ? "<center>True</center>" : "<center>False</center>" } },
			{ field: "Verified", title: "Verified Status", template: function (dataItem) { return dataItem.Verified == 1 ? "<center>True</center>" : "<center>False</center>" } },
			{ field: "WarningCount", title: "Warning Count", template: function (dataItem) { return dataItem.VIP == 0 && dataItem.WarningCount >= 3 ? "<center>" + dataItem.WarningCount + "<span style='margin-left: 5px; color: red;' title='This user should be suspended!' class='glyphicon glyphicon-exclamation-sign'></span></center>" : "<center>" + dataItem.WarningCount + "</center>" } },
			{ field: "MoneySpent", title: "Money Spent", template: function (dataItem) { return dataItem.MoneySpent > 100 && dataItem.VIP == 0 && dataItem.DemotionCount == 0 ? "<center>" + '$' + dataItem.MoneySpent.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "<span style='margin-left: 5px; color: green;' title='This user should be promoted!' class='glyphicon glyphicon-exclamation-sign'></span></center>" : "<center>" + '$' + dataItem.MoneySpent.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "</center>" } },
            { command: { text: "Verify User", click: Verify }, title: "Verify" },
			{ command: { text: "Promote to VIP", click: PromoteUser }, title: "Promote" },
			{ command: { text: "Issue Warning", click: IssueWarning }, title: "Warn" },
			{ command: ["destroy"], title: "Suspend" }
		],
		messages:
		{

			commands: {
				destroy: "Suspend User"
			}
		},
		
		editable:
		{
			confirmation: true,
			confirmDelete: "Yes",
			mode: "popup",
			createAt: "top"

		},

	});

	function PromoteUser(e)
	{
		var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
		if (dataItem.WarningCount >= 3) {
			alert("This user's warnings exceed 3. They must be suspended.");
			return;
		}
		console.log(dataItem);
		$.ajax({
			url: '/Manager/PromoteUser',
			type: "POST",
			async: false,
			data: { 'UserID': dataItem.ID },
			success: function (result) {
				location.reload();
			}
		})
	}

	function IssueWarning(e){
		var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
		if (dataItem.WarningCount >= 3)
		{
			alert("This user's warnings exceed 3. They must be suspended.");
			return;
		}
		console.log(dataItem);
		$.ajax({
			url: '/Manager/IssueWarning',
			type: "POST",
			async: false,
			data: { 'UserID': dataItem.ID },
			success: function (result) {
				location.reload();
			}
		})
	}

	function Verify(e) {
		var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
		if (dataItem.WarningCount >= 3) {
			alert("This user's warnings exceed 3. They must be suspended.");
			return;
		}

		console.log(dataItem); 
		if (dataItem.Verified == 1)
		{
			alert("This user is already verified.");
			return;

		}
		console.log(dataItem);
		$.ajax({
			url: '/Manager/Verify',
			type: "POST",
			async: false,
			data: { 'UserID': dataItem.ID },
			success: function (result) {
				location.reload();
			}
		})
	}

	function detailInit(e) {
		console.log('current row', e); 
		$("<div/>").appendTo(e.detailCell).kendoGrid({
			dataSource: {
				transport: {
					read: function (options) {
						$.ajax({
							url: '/Manager/GetUserDetails',
							type: "GET",
							async: false,
							data: { 'UserID': e.data.ID },
							success: function (result) {
								console.log('data', result);
								options.success(result);
							}
						})
					}
				},

				schema: {
					model: {
						id: "ID",
						fields: {
							Name: {type : "string"}, 
							PreparedBy: { type: "string" },
							Rating: { type: "number" },
							Approval: { type: "number" },
							Comment: { type: "string" }
						}
					}
				}

			},

			columns:
				[
					{ field: "Name", title: "Food Name", template: function (dataItem) { return "<center>" + dataItem.Name + "</center>" } },
					{ field: "PreparedBy", title: "Prepared By", template: function (dataItem) { return "<center>" + dataItem.PreparedBy + "</center>" } },
					{ field: "Rating", title: "Rating", template: function (dataItem) { return "<center>" + dataItem.Rating + "</center>" } },
					{ field: "Approval", title: "Approval", template: function (dataItem) { return "<center>" + dataItem.Approval + "</center>" } },
					{ field: "Comment", title: "Comment" }
				]
		}); 

	}
});