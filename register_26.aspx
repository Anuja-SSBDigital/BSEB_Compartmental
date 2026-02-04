<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="register_26.aspx.cs" Inherits="register_26" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="card">
        <div class="card-header">
            <h4>Student Registration</h4>
        </div>
        <div class="card-body">
            <div class="form-horizontal">
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label for="location" class="col-form-label">Location</label>
                        <select class="form-control" id="location">
                            <option selected>R.L. COLLEGE, NIMATHI, DARBHANGA | 51029</option>
                        </select>
                    </div>
                    <div class="form-group col-md-6">
                        <label for="faculty" class="col-form-label">Faculty</label>
                        <select class="form-control" id="faculty">
                            <option selected>ARTS</option>
                        </select>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-sm-2 col-form-label">Registration Mode</label>
                    <div class="col-sm-10">
                        <div class="form-check form-check-inline">
                            <input class="form-check-input" type="radio" name="regMode" id="nonOfss" value="non-ofss">
                            <label class="form-check-label" for="nonOfss">NON-OFSS DATA</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <input class="form-check-input" type="radio" name="regMode" id="ofss" value="ofss">
                            <label class="form-check-label" for="ofss">OFSS REGISTRATION LIST</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <input class="form-check-input" type="radio" name="regMode" id="registered" checked>
                            <label class="form-check-label" for="registered">DISPLAY REGISTERED LIST</label>
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-sm-2 col-form-label">Registration Type</label>
                    <div class="col-sm-10">
                        <div class="form-check form-check-inline">
                            <input class="form-check-input" type="radio" name="regType" id="regular" checked>
                            <label class="form-check-label" for="regular">REGULAR</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <input class="form-check-input" type="radio" name="regType" id="private">
                            <label class="form-check-label" for="private">PRIVATE</label>
                        </div>
                    </div>
                </div>

                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label class="col-form-label">Available Seat in Regular</label>
                        <input type="text" class="form-control" value="44" readonly>
                    </div>
                    <div class="form-group col-md-6">
                        <label class="col-form-label">Available Seat in Private</label>
                        <input type="text" class="form-control" value="192" readonly>
                    </div>
                </div>

                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label class="col-form-label">Search Student by Name</label>
                        <input type="text" class="form-control" placeholder="Enter Name">
                    </div>
                    <div class="form-group col-md-6">
                        <label class="col-form-label">Search Student by OFSS Reference no.</label>
                        <input type="text" class="form-control" placeholder="Enter OFSS Reference No.">
                    </div>
                </div>
            </div>

            <!-- Result Summary -->
            <div class="info-box mt-4 text-center">
                <p class="text-dark">Total No. of Student Payment Done: <strong>340</strong></p>
                <p class="text-success">Total No. of Student Form Submitted: <strong>340</strong></p>
                <p class="text-danger">Payment done but Form not Submitted: <strong>340</strong></p>
            </div>

            <div class="table-responsive">
                <table class="table table-bordered table-striped table-sm">
                    <thead>
                        <tr>
                            <th>S. No.</th>
                            <th>College Code</th>
                            <th>Student Name</th>
                            <th>Father Name</th>
                            <th>Mother Name</th>
                            <th>Year Of Passing</th>
                            <th>Click to Register</th>
                            <th>View</th>
                            <th>Delete</th>
                        </tr>
                    </thead>
                    <!-- Same head and form as before -->
                    <!-- Only the <tbody> of table is shown below with 10 students -->
                    <tbody>
                        <tr>
                            <td>1</td>
                            <td>51029</td>
                            <td>FARHEEN FATMA</td>
                            <td>MD SHUAIB</td>
                            <td>AFSANA KHATOON</td>
                            <td>2024</td>
                            <td></td>
                            <td>
                                <button class="btn btn-info btn-sm">View</button></td>
                            <td>
                                <button class="btn btn-danger btn-sm">Delete</button></td>
                        </tr>
                        <tr>
                            <td>2</td>
                            <td>51029</td>
                            <td>BRAHAMANAND PANDIT</td>
                            <td>RUDAL PANDIT</td>
                            <td>SANGITA DEVI</td>
                            <td>2024</td>
                            <td></td>
                            <td>
                                <button class="btn btn-info btn-sm">View</button></td>
                            <td>
                                <button class="btn btn-danger btn-sm">Delete</button></td>
                        </tr>
                        <tr>
                            <td>3</td>
                            <td>51029</td>
                            <td>MD SARFRAJ ALAM</td>
                            <td>MD MURTUJA ANSARI</td>
                            <td>SABANA BEGAM</td>
                            <td>2024</td>
                            <td></td>
                            <td>
                                <button class="btn btn-info btn-sm">View</button></td>
                            <td>
                                <button class="btn btn-danger btn-sm">Delete</button></td>
                        </tr>
                        <tr>
                            <td>4</td>
                            <td>51029</td>
                            <td>ALTAF HUSSAIN</td>
                            <td>MD ASFAK</td>
                            <td>KHAIRUN NISHA</td>
                            <td>2024</td>
                            <td></td>
                            <td>
                                <button class="btn btn-info btn-sm">View</button></td>
                            <td>
                                <button class="btn btn-danger btn-sm">Delete</button></td>
                        </tr>
                        <tr>
                            <td>5</td>
                            <td>51029</td>
                            <td>ANAND KUMAR</td>
                            <td>SHAMBHU YADAV</td>
                            <td>ASHA DEVI</td>
                            <td>2024</td>
                            <td></td>
                            <td>
                                <button class="btn btn-info btn-sm">View</button></td>
                            <td>
                                <button class="btn btn-danger btn-sm">Delete</button></td>
                        </tr>
                        <tr>
                            <td>6</td>
                            <td>51029</td>
                            <td>ARCHNA KUMARI</td>
                            <td>RAMSEVAK SAH</td>
                            <td>MUNNI DEVI</td>
                            <td>2024</td>
                            <td></td>
                            <td>
                                <button class="btn btn-info btn-sm">View</button></td>
                            <td>
                                <button class="btn btn-danger btn-sm">Delete</button></td>
                        </tr>
                        <tr>
                            <td>7</td>
                            <td>51029</td>
                            <td>SONU KUMAR SINGH</td>
                            <td>SANJAY SINGH</td>
                            <td>KHUSHBOO DEVI</td>
                            <td>2024</td>
                            <td></td>
                            <td>
                                <button class="btn btn-info btn-sm">View</button></td>
                            <td>
                                <button class="btn btn-danger btn-sm">Delete</button></td>
                        </tr>
                        <tr>
                            <td>8</td>
                            <td>51029</td>
                            <td>KHUSHBOO KUMARI</td>
                            <td>SANJAY PASWAN</td>
                            <td>DUKHNI DEVI</td>
                            <td>2024</td>
                            <td></td>
                            <td>
                                <button class="btn btn-info btn-sm">View</button></td>
                            <td>
                                <button class="btn btn-danger btn-sm">Delete</button></td>
                        </tr>
                        <tr>
                            <td>9</td>
                            <td>51029</td>
                            <td>KHUSHBOO KUMARI</td>
                            <td>SANJIT SAH</td>
                            <td>GEETA DEVI</td>
                            <td>2024</td>
                            <td></td>
                            <td>
                                <button class="btn btn-info btn-sm">View</button></td>
                            <td>
                                <button class="btn btn-danger btn-sm">Delete</button></td>
                        </tr>
                        <tr>
                            <td>10</td>
                            <td>51029</td>
                            <td>MD HIFZUR RAHMAN</td>
                            <td>SAMASE ALAM</td>
                            <td>SAYARA BANO</td>
                            <td>2024</td>
                            <td></td>
                            <td>
                                <button class="btn btn-info btn-sm">View</button></td>
                            <td>
                                <button class="btn btn-danger btn-sm">Delete</button></td>
                        </tr>
                    </tbody>

                </table>
            </div>
        </div>
    </div>
</asp:Content>
