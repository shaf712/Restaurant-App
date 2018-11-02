var food_type = [{ TypeID: 1, TypeName: 'Breakfast' }, { TypeID: 2, TypeName: 'Lunch' }, { TypeID: 3, TypeName: 'Dinner' }];
var chefs = [];

function GetChefs() {
	$.ajax({
		url: "/Chef/GetChefs",
		type: "get",
		async: false,
		success: function (result) {
			chefs = result;
		},
	}); 
}

$(document).ready(function () {
		GetChefs();
    	$("#menu").kendoGrid({
        		dataSource: {
        			transport:
					{
						read: function(options)
						{
							console.log(options); 
							options.success(menu); 
						},
						create: function(options)
						{
							options.data.TypeName = GetFoodTypeID($("#typeDDL").val());
							options.data.ChefID = $("#chefDDL").val();
							options.data.FullName = GetChefName(options.data.ChefID); 
							console.log(options.data.ChefID); 
							console.log(options.data);
							$.ajax
							({
								url: '/Chef/Create',
								type: "POST",
								async: false,
								contentType: "application/json;",
								data: JSON.stringify(options.data),
								success: function(result)
								{
									options.data.ID = result;
									options.data.FullName = GetChefName($("#chefDDL").val());
									console.log(options.data.FullName); 
									menu.unshift(options.data);
									options.success(options.data);
									location.reload(); 
								}
							}); 
						},
						//update: function(options)
						//{
                        //    console.log('updating!'); 
						//	options.data.TypeName = GetFoodTypeID($("#typeDDL").val());
						//	options.data.ChefID = $("#chefDDL").val();
						//	console.log(options.data); 
						//	$.ajax
						//	({
						//		url: '/Chef/Update',
						//		type: "POST",
						//		async: false,
						//		contentType: "application/json;",
						//		data: JSON.stringify(options.data),
						//		success: function (result) {
						//			options.success(options.data);
						//		}
						//	});
						//},
						destroy: function(options)
						{
							console.log(options.data); 
							$.ajax
							({
								url: '/Chef/Delete',
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
								Name: { type: "string" },
								TypeID: { type: "number"}, 
								Description: { type: "string" },
								Price: { type: "number"}, 
								Image: { type: "string" },
								ChefID: {type: "number"} 
							}
        				}
        			}
        		},

        		columns:
				[
					{ field: "Name", title: "Food Name", template: function (dataItem) { return "<center>" + dataItem.Name + "</center>" } },
					{ field: "ChefID", title: "Prepared By", editor: ChefEditor, width: 150, template: function (dataItem) { return "<center>" + dataItem.PreparedBy + "</center>"; } },
					{ field: "Image", title: "Food Image",
						template: function(dataItem)
						{
							return "<center><img src='" + dataItem.Image + "' height='300'; width='350' alt='No Image Found' ></img><center>";
						},
						editor: function (container, options)
						{
							$(container).parent().parent().parent().css("width", "500px");
							$(container).parent().css("width", "500px");
							$("<div id='cropper' style='margin-right: 0px; height: 300px; width: 350px;'>" +
							"<img src='" +
							options.model.Image +
							"' style='height: 300px;'/>" +
							"<input type='file' id='Imagecropper' name='Imagecropper' accept='image/*' />" +
							"</div>")
							.appendTo(container);

							$('#cropper').slim(
							{
								"label": "Click to add image",
								"ratio": "1:1",
								"edit": true,
								"size": {
									"width": 200,
									"height": 200
								},
								"download": false,
								"push": true,
								"willSave": function (data, ready) {
									var dataItem = $("#menu").data("kendoGrid").dataSource.getByUid(options.model.uid);
									dataItem.dirty = true;
									options.model.Image = data.output.image;
									$('#ImgVal').val(data.output.image);
									ready(data);
								},
								"willRemove": function imageWillBeRemoved(slim, remove) {
									var dataItem = $("#menu").data("kendoGrid").dataSource.getByUid(options.model.uid);
									dataItem.dirty = true;
									options.model.Image = "";
									$('#ImgVal').val("");
									remove();
								}
							});
							$('<input id="ImgVal" style="display: none;" type="text" name="Image" required="required" data-bind="value:' + options.field + '"/>')
							.appendTo(container);
						},
						width: 400
					}, 
					{ field: "Description", title: "Description",
						template: function (dataItem) { return "<center>" + dataItem.Description + "</center>"; },
						editor: function (container, options)
						{
							var kendoEditor = $('<span data-for="Description" class="k-invalid-msg"></span><textarea style="height: 10px!important" required="required" novalidate="novalidate" data-maxtextlength="100" data-maxtextlength-msg="Text must be shorter than 100 chars" data-bind="value:' + options.field + '"/> ').css("width","100%").appendTo(container);
							kendoEditor.kendoEditor({
								tools: [], 
								resizable: {
									content: false,
									toolbar: false
								},
							}); 
						}
					},
					{ field: "TypeID", title: "Type", editor: TypeEditor, width: 150, template: function (dataItem) { return "<center>" + dataItem.TypeName + "</center>"; } },
					{ field: "Price", title: "Price", template: function (dataItem) { kendo.template($("#priceTemplate").html()); return "<center>" + '$' + dataItem.Price.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,') + "</center>"; } },
					{ command: ["destroy"], title: "Delete Food Item" }
				],
					messages: 
        			{
        				commands: { 
        					update: "Save Changes",
							create: "Add Food Item"
        				}
        			}, 
					toolbar: ["create"],
					filterable: true, 
					editable: 
					{
						confirmation: true, 
						confirmDelete: "Yes", 
						mode: "popup", 
						createAt: "top"
					}
        	});

        	function TypeEditor(typeContainer, typeOptions)
        	{
					$('<input type="text"  id="typeDDL" name="Type" style="width: 100%; font-family: Arial;" required="required" data-bind="value:' + typeOptions.field + '"/>')
					.css("width", "100%")
					.appendTo(typeContainer)
					.kendoDropDownList({
						dataTextField: "TypeName",
						dataValueField: "TypeID",
						dataSource: food_type
					});
        	}

        	function ChefEditor(chefContainer, chefOptions) {
        		$('<input type="text"  id="chefDDL" name="Type" style="width: 100%; font-family: Arial;" required="required" data-bind="value:' + chefOptions.field + '"/>')
				.css("width", "100%")
				.appendTo(chefContainer)
				.kendoDropDownList({
					dataTextField: "FullName",
					dataValueField: "ID",
					dataSource: chefs
				});
        	}

        	function GetFoodTypeID(obj) {
        		console.log(obj); 
        		for (i = 0; i < food_type.length; i++)
        			if (obj == food_type[i].TypeID)
        				return food_type[i].TypeName; 
        	}

        	function GetChefName(obj) {
        		console.log('chef ID', obj);
        		for (i = 0; i < chefs.length; i++)
        			if (obj == chefs[i].ID)
        				return chefs[i].FullName;
        	}
     });
