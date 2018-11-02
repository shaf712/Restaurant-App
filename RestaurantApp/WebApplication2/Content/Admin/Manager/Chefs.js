$(document).ready(function () {
	$("#ChefGrid").kendoGrid({
		dataSource: {
			transport:
			{
				read: function (options) {
					console.log(options);
					options.success(chefs);
				},
				create: function (options) {
					var FullName = options.data.FullName.split(' '); 
					options.data.FirstName = FullName[0];
					options.data.LastName = FullName[1];
					console.log(options.data);
					console.log(options.data.Salary.toString()); 
					$.ajax
					({
						url: '/Manager/AddChef',
						type: "POST",
						async: false,
						contentType: "application/json;",
						data: JSON.stringify(options.data),
						success: function (result) {
							options.data.ID = result;
							options.data.Approval = 0;
							options.data.DemotionCount = 0;
							options.data.PromotionCount = 0; 
							chefs.unshift(options.data);
							options.success(options.data);
						}
					});
				},
				destroy: function (options) {
					console.log(options.data);
					$.ajax
					({
						url: '/Manager/FireChef',
						type: "POST",
						data: { 'ChefID': options.data.ID },
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
						FullName: { type: "string" },
						Salary: { type: "number" },
						Approval: { type: "number" },
						DemotionCount: {type: "number"}, 
						PromotionCount: {type: "number"}, 
						LastOrder: {type: "string"} 
					}
				}
			}
		},

		columns:
		[
			{ field: "FullName", title: "Name", template: function (dataItem) { return "<center>" + dataItem.FullName + "</center>" } },
			{ field: "Salary", title: "Annual Salary", template: function (dataItem) { return "<center>" + '$' + dataItem.Salary.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "</center>"; } },
			{ field: "Approval", title: "Approval",  template: function (dataItem) { return dataItem.Approval <= -3 ? "<center>" + dataItem.Approval + "<span style='margin-left: 5px; color: red;' title='This chef has received consistent low ratings! Demotion Recommended!' class='glyphicon glyphicon-exclamation-sign'></span></center>" : dataItem.Approval >= 3 ? "<center>" + dataItem.Approval + "<span style='margin-left: 5px; color: green;' title='Promotion Recommended!' class='glyphicon glyphicon-exclamation-sign'></span></center>" : "<center>" + dataItem.Approval + "</center>"; } },
			{ field: "DemotionCount", title: "Demotion Count", template: function (dataItem) { return dataItem.DemotionCount >= 2 ? "<center>" + dataItem.DemotionCount + "<span style='margin-left: 5px; color: red;' title='This chef has been demoted twice already!' class='glyphicon glyphicon-exclamation-sign'></span></center>" : "<center>" + dataItem.DemotionCount + "</center>"; } },
			{ field: "PromotionCount", title: "Promotion Count", template: function (dataItem) { return "<center>" + dataItem.PromotionCount + "</center>"; } },
			{ field: "LastOrder", title: "Last Order Date", template: function (dataItem) { return DateCompare(dataItem.LastOrder) == 0 ? "<center>" + dataItem.LastOrder + "</center>" : "<center>" + dataItem.LastOrder + "<span style='margin-left: 5px; color: red;' title='This chef has not received an order in the past 3 days!' class='glyphicon glyphicon-exclamation-sign'></span></center"; } },
			{ command: { text: "Promote", click: PromoteChef } },
			{ command: { text: "Demote", click: DemoteChef }},
			{ command: ["destroy"], title: "Fire" }
		],
		messages:
		{
			commands: {
				destroy: "Fire Worker",
				create: "Add New Employee"
			}
		},
		edit: function(e){
			e.container.find("[data-container-for='Approval']").hide();
			e.container.find($("label[for='Approval']")).hide();
			e.container.find("[data-container-for='DemotionCount']").hide();
			e.container.find($("label[for='DemotionCount']")).hide();
			e.container.find("[data-container-for='PromotionCount']").hide();
			e.container.find($("label[for='PromotionCount']")).hide();
			e.container.find("[data-container-for='LastOrder']").hide();
			e.container.find($("label[for='LastOrder']")).hide();

		}, 
		toolbar: ["create"],
		editable:
		{
			confirmation: true,
			confirmDelete: "Yes",
			mode: "popup",
			createAt: "top"
		}, 

	});


	function DateCompare(date)
	{
		var order_date = moment(date).format('DD');
		var today = new Date();
		var dd = today.getDate();
		order_date = parseInt(order_date);
		if (order_date.isNan) {
			return 2;
		}
		dd = parseInt(dd); 
		if ((dd - order_date) >= 3) {
			return 1;
		}

		return 0; 
	}

	function PromoteChef(e) {
		var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
		$.ajax({
			url: '/Manager/Promote',
			type: "POST",
			async: false,
			data: { 'ChefID': dataItem.ID },
			success: function(result)
			{
				location.reload(); 
			}
		})
	}

	function DemoteChef(e) {
		var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
		if (dataItem.DemotionCount >= 2)
		{
			alert('Cannot demote anymore. Worker must be fired.');
			$("#demotion-button").hide(); 
			return; 
		}
		$.ajax({
			url: '/Manager/Demote',
			type: "POST",
			async: false,
			data: { 'ChefID': dataItem.ID },
			success: function (result) {
				location.reload();
			}
		})
	}


});