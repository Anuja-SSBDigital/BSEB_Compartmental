<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StudentPhotoAndsignatureDetails.aspx.cs" Inherits="StudentPhotoAndsignatureDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <h4>Add Photo Signature</h4>
                </div>
                <div class="card-body">
                    <div id="showimageandsign">
                        <!-- Photo Upload -->
                        <div class="mb-4">
                            <asp:Image ID="impPhoto" runat="server" CssClass="preview-img mb-2"
                                AlternateText="Photo Preview" Width="120px" Height="140px" ClientIDMode="Static" />
                            <br />
                            <asp:FileUpload ID="stuPhoto" runat="server" CssClass="form-control"
                                accept=".jpg,.jpeg" onchange="ShowPreviewPhoto(this)" />
                            <small class="text-muted">[Photo (Passport) file size should not be more than 100 KB and supported file types are .jpg, .jpeg]</small><br />
                            <span id="photoError" class="text-danger" role="alert" style="display: none;"></span>
                        </div>

                        <!-- Signature Upload -->
                        <div class="mb-4">
                            <asp:Image ID="imgSign" runat="server" CssClass="preview-img mb-2"
                                AlternateText="Signature Preview" Width="120px" Height="50px" ClientIDMode="Static" />
                            <br />
                            <asp:FileUpload ID="stuSignature" runat="server" CssClass="form-control"
                                accept=".jpg,.jpeg" onchange="ShowPreviewSignature(this)" />
                            <small class="text-muted">[Signature file size should not be more than 20 KB and supported file types are .jpg, .jpeg]</small><br />
                            <span id="signatureError" class="text-danger" role="alert" style="display: none;"></span>
                        </div>

                        <div class="col-12 text-center">
                            <asp:HiddenField ID="hfFacultyId" runat="server" />
                            <asp:HiddenField ID="hfCategoryType" runat="server" />
                            <asp:HiddenField ID="hfExistingPhotoPath" runat="server" />
                            <asp:HiddenField ID="hfExistingSignaturePath" runat="server" />
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary px-4 mt-2"
                                OnClientClick="return validateForm();" OnClick="btnUpdate_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        // Function to show preview of the selected photo
        function ShowPreviewPhoto(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var image = document.getElementById("impPhoto");
                    image.src = e.target.result;
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        // Function to show preview of the selected signature
        function ShowPreviewSignature(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var image = document.getElementById("imgSign");
                    image.src = e.target.result;
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        // Validate the form before submitting
        function validateForm() {
            debugger
            var isValid = true;

            // Get the elements for validation
            var photoInput = document.getElementById('<%= stuPhoto.ClientID %>');
         var photoError = document.getElementById('photoError');
         var signInput = document.getElementById('<%= stuSignature.ClientID %>');
    var signatureError = document.getElementById('signatureError');

    // Reset error messages
    photoError.style.display = 'none';
    signatureError.style.display = 'none';

    // Validate the Photo
    var photoFile = photoInput.files[0];
    var existingPhotoPath = document.getElementById('<%= hfExistingPhotoPath.ClientID %>').value;

    // Check if a new photo is uploaded, OR if there's no new photo AND no existing one.
    if (photoFile) {
        var photoSize = photoFile.size / 1024; // Convert size to KB
        if (photoSize > 100) {
            photoError.textContent = 'Photo size should not exceed 100 KB.';
            photoError.style.display = 'inline';
            isValid = false;
        } else if (!photoFile.type.match(/image\/jpeg$/i)) {
            photoError.textContent = 'Invalid file type. Please upload .jpg or .jpeg.';
            photoError.style.display = 'inline';
            isValid = false;
        }
    } else if (!existingPhotoPath) {
        photoError.textContent = 'Please upload a photo.';
        photoError.style.display = 'inline';
        isValid = false;
    }

    // Validate the Signature
    var signatureFile = signInput.files[0];
         var existingSignaturePath = document.getElementById('<%= hfExistingSignaturePath.ClientID %>').value;

         // Check if a new signature is uploaded, OR if there's no new signature AND no existing one.
         if (signatureFile) {
             var signSize = signatureFile.size / 1024; // Convert size to KB
             if (signSize > 20) {
                 signatureError.textContent = 'Signature size should not exceed 20 KB.';
                 signatureError.style.display = 'inline';
                 isValid = false;
             } else if (!signatureFile.type.match(/image\/jpeg$/i)) {
                 signatureError.textContent = 'Invalid file type. Please upload .jpg or .jpeg.';
                 signatureError.style.display = 'inline';
                 isValid = false;
             }
         } else if (!existingSignaturePath) {
             signatureError.textContent = 'Please upload a signature.';
             signatureError.style.display = 'inline';
             isValid = false;
         }

         return isValid; // Only submit the form if everything is valid
     }
    </script>
</asp:Content>
