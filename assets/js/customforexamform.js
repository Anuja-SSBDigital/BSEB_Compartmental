

        function updateAndPrint() {
                const urlParams = new URLSearchParams(window.location.search);
        const studentData = urlParams.get("studentData");

        if (studentData) {
            $.ajax({
                type: "POST",
                url: "ExaminationForm.aspx/UpdateDownloaded",
                data: JSON.stringify({ studentData: studentData }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d && response.d.startsWith("error:")) {
                        alert("Update failed: " + response.d);  // 🔔 Show server error
                    } else {
                        console.log("Update result:", response.d);
                    }
                    window.print();
                },
                error: function (xhr, status, error) {
                    alert("AJAX error: " + error); // 🔔 Show client error
                    window.print();
                }
            });
                } else {
            window.print();
                }
            }
    