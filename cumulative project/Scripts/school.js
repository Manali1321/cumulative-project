window.onload = function () {
    var formHandle = document.forms.teacherform;
    formHandle.onsubmit = processForm;
    function processForm() {
        var fname = formHandle.TeacherFname;
        var lname = formHandle.TeacherLname;
        var hiredate = formHandle.HireDate;
        var salary = formHandle.Salary;
        var employeenumber = formHandle.EmployeeNumber;
        var id = /(t|T)\d{3}/;

        if (fname.value === "") {
            nameMsg = document.getElementById("TFname")
            nameMsg.style.background = "red";
            fname.focus();
            return false;
        }
        if (lname.value === "") {
            nameMsg = document.getElementById("TLname")
            nameMsg.style.background = "red";
            lname.focus();
            return false;
        }
        if (hiredate.value === "") {
            nameMsg = document.getElementById("Hdate")
            nameMsg.style.background = "red";
            hiredate.focus();
            return false;
        }
        if (salary.value === "") {
            nameMsg = document.getElementById("Spay")
            nameMsg.style.background = "red";
            salary.focus();
            return false;
        }
        if (!id.test(employeenumber.value) || employeenumber.value === "") {
            nameMsg = document.getElementById("Enumber")
            nameMsg.style.background = "red";
            employeenumber.focus();
            return false;
        }
    }
}
