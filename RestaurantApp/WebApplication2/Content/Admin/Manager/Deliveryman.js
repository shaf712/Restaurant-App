$(document).ready(function () {
	$("#deliverymanGrid").kendoGrid({
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
						url: '/Manager/AddDeliveryman',
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
						url: '/Manager/FireDeliveryman',
						type: "POST",
						data: { 'ID': options.data.ID },
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
			{ field: "Approval", title: "Approval",  template: function (dataItem) { return dataItem.Approval <= -3 ? "<center>" + dataItem.Approval + "<span style='margin-left: 5px; color: red;' title='This deliveryman has received consistent low ratings! Demotion Recommended!' class='glyphicon glyphicon-exclamation-sign'></span></center>" : dataItem.Approval >= 3 ? "<center>" + dataItem.Approval + "<span style='margin-left: 5px; color: green;' title='Promotion Recommended!' class='glyphicon glyphicon-exclamation-sign'></span></center>" : "<center>" + dataItem.Approval + "</center>"; } },
			{ field: "DemotionCount", title: "Demotion Count", template: function (dataItem) { return dataItem.DemotionCount >= 2 ? "<center>" + dataItem.DemotionCount + "<span style='margin-left: 5px; color: red;' title='This deliveryman has been demoted twice already!' class='glyphicon glyphicon-exclamation-sign'></span></center>" : "<center>" + dataItem.DemotionCount + "</center>"; } },
			{ field: "PromotionCount", title: "Promotion Count", template: function (dataItem) { return "<center>" + dataItem.PromotionCount + "</center>"; } },
			{ command: { text: "Promote", click: PromoteDeliveryman } },
			{ command: { text: "Demote", click: DemoteDeliveryman } },
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


	function PromoteDeliveryman(e) {
		var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
		$.ajax({
			url: '/Manager/PromoteDeliveryman',
			type: "POST",
			async: false,
			data: { 'ID': dataItem.ID },
			success: function(result)
			{
				location.reload(); 
			}
		})
	}

	function DemoteDeliveryman(e) {
		var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
		if (dataItem.DemotionCount >= 2)
		{
			alert('Cannot demote anymore. Worker must be fired.');
			$("#demotion-button").hide(); 
			return; 
		}
		$.ajax({
			url: '/Manager/DemoteDeliveryman',
			type: "POST",
			async: false,
			data: { 'ID': dataItem.ID },
			success: function (result) {
				location.reload();
			}
		})
	}


});