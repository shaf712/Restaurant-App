
$(document).ready(function () {
	var self = {};
	self.CurrentItem = ko.observable(); 
	self.Purchased = ko.mapping.fromJS(menuViewModel);
	//console.log(self.Breakfast()[0].Name()); 

	self.GetItem = function(data)
	{
		self.CurrentItem(data); 
		$("#myModal").modal('show'); 
		
	}

	ko.applyBindings(self);

	self.Save = function (data) {
		var complaint = $('#complaint-radio').prop('checked');
		console.log(complaint);

		var rating = $("#rate-select").children(":selected").attr("id");

		var delivery_rating = $("#delivery-rate-select").children(":selected").attr("id");

		if (rating == 0 || delivery_rating == 0)
		{
			alert('You must enter a rating for this food item.');
			return; 
		}
		
		console.log(data);
		console.log('VVIP status:', isVVIP); 
		$.ajax({
			url: '/Item/SubmitReview',
			type: "POST",
			async: false,
			data: { 'UserID': UserID, 'isVVIP': isVVIP, 'ItemID': data.ID(), 'ChefID': data.ChefID(), 'DeliverymanID': data.DeliverymanID(), 'Rating': rating, 'DeliveryRating': delivery_rating, 'Approval': complaint ? -1 : 1, 'Comment': $("#comment-section").val() },
			success: function(result)
			{
				$("#comment-success").removeClass('hide');
				$("#close-modal").addClass('disabled');
				setTimeout(function () { $("#myModal").modal('hide'); }, 1200);
			}
		})



	}
})