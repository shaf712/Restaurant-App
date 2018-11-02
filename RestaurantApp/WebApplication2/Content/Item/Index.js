$(document).ready(function () {
	var self = this;
	self.Item = ko.mapping.fromJS(FoodItem);




	console.log(self.Item);
	ko.applyBindings(self);

	var count = 0;
	
	$('*[class^="glyphicon glyphicon-star-empty"]').each(function (i, obj) {
		if (count < self.Item.Rating()) {
			obj.className = obj.className.replace('glyphicon-star-empty', 'glyphicon-star');
			count++; 
		}
	})
	//var self = {};
	//self.Breakfast = ko.mapping.fromJS(Breakfast);
	//self.Lunch = ko.mapping.fromJS(Lunch);
	//self.Dinner = ko.mapping.fromJS(Dinner);
	////console.log(self.Breakfast()[0].Name()); 

})