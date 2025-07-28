
    // Get the current date
    var currentDate = new Date();
    
    // Format the date
    var dateString = currentDate.toLocaleDateString("en-US", {
     
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
    
    // Display the formatted date
    document.getElementById("date").innerHTML = dateString;

  // datepikar

    const datePicker = document.getElementById('datepicker');
    
    datePicker.addEventListener('change', function() {
        const selectedDate = datePicker.value;
        console.log("Selected Date:", selectedDate);

//   Status tracking

var close ;

function dot(){
    document.getElementsByClassName("btn").innerHTML="kjhskjfd"
}

function ShowOrderStatus() {
  document.getElementById("Cutting").innerHTML = "table content <button class='btn' onClick='dot()' style='background-color:blue;'></button>";
}

function StretchingStatus() {
  document.getElementById("Stretching").innerHTML = "second content ";
}


function IroningStatus() {
    document.getElementById("Ironing").innerHTML = "ironing content";
  }

  
function DeliveryStatus() {
    document.getElementById("Delivery").innerHTML = "Delivery content";
  }




    function validateForm() {
        const name =
            document.forms.RegForm.Name.value;
        const address =
            document.forms.RegForm.Address.value;

        const AccountNumber =
            document.forms.RegForm.AccountNumber.value;


        const pfNumber =
            document.forms.RegForm.pfNumber.value;

        const IFSC =
            document.forms.RegForm.IFSC.value;

        const Department =
            document.forms.RegForm.Department.value;

        const EmployeeType =
            document.forms.RegForm.EmployeeType.value;

        EmployeeType

        const email =
            document.forms.RegForm.EMail.value;

        //Javascript Regex for Email Validation.
        const regEmail =
            /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/g;

        // Javascript Regex for Phone Number validation.
        const regPhone = /^\d{10}$/;

        // Javascript Regex for Name validation
        const regName = /\d+$/g;

        if (name == "" || regName.test(name)) {
            window.alert
                ("Please enter your name properly.");
            name.focus();
            return false;
        }

        if (address == "") {
            window.alert
                ("Please enter your address.");
            address.focus();
            return false;
        }


        if (IFSC == "") {
            alert("Please enter your IFSC");
            IFSC.focus();
            return false;
        }

        if (IFSC.length < 6) {
            alert
                ("IFSC should be atleast 6 character long");
            IFSC.focus();
            return false;

        }

        if (pfNumber == "" || !regPhone.test(pfNumber)) {
            alert("Please enter valid PF  number.");
            pfNumber.focus();
            return false;
        }

        if (AccountNumber == "" || !regPhone.test(AccountNumber)) {
            alert("Please enter valid phone number.");
            AccountNumber.focus();
            return false;
        }

        if (Department.selectedIndex == -1) {
            alert("Pleas Select your Department.");
            Department.focus();
            return false;
        }

        if (EmployeeType.selectedIndex == -1) {
            alert("Pleas Select your EmployeeType.");
            EmployeeType.focus();
            return false;
        }


        if (email == "" || !regEmail.test(email)) {
            window.alert
                ("Please enter a valid e-mail address.");
            email.focus();
            return false;
        }


        return true;
    }
    // Get the current date