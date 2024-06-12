import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:flutter_multi_formatter/formatters/masked_input_formatter.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/roles.dart';
import 'package:ui/helpers/dateTime_helper.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/models/user.dart';
import 'package:ui/services/auth_storage_provider.dart';
import 'package:ui/services/user_provider.dart';

class InsertUserModal extends StatefulWidget {
  const InsertUserModal(
      {super.key,
      required this.onCompleted,
      required this.userTypeDropdown,
      required this.agenciesDropdown,
      this.user});

  final Function onCompleted;
  final User? user;
  final List<DropdownModel>? agenciesDropdown;
  final List<Map<Roles?, String>>? userTypeDropdown;
  @override
  _InsertUserModalState createState() => _InsertUserModalState();
}

class _InsertUserModalState extends State<InsertUserModal> {
  final _formKey = GlobalKey<FormBuilderState>();
  Map<String, dynamic> _initialValue = {};

  UserProvider? _userProvider;

  Roles? selectedUserType;
  String? selectedAgency;
  
  @override
  void initState() {
    super.initState();

    if (widget.user != null) {
      _initialValue = {
        "firstName": widget.user?.firstName,
        "lastName": widget.user?.lastName,
        "birthDate":
            DateTimeHelper.formatDate(widget.user?.birthDate, "dd-MM-yyyy"),
        "residencePlace": widget.user?.residencePlace,
        "email": widget.user?.email,
        "phoneNumber": widget.user?.phoneNumber,
      };
      selectedUserType = RoleEnumManager.getRoleById(widget.user!.role);
      selectedAgency = widget.user?.agency?.id;
    }

    if (AuthStorageProvider.getAuthData()?["role"] == Roles.coordinator){
        selectedAgency = AuthStorageProvider.getAuthData()?["agencyId"];
        selectedUserType = Roles.touristGuide;
    }
    
    _userProvider = context.read<UserProvider>();
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: Column(
        children: [
          Text(widget.user == null ? 'Dodaj korisnika' : "Uredi korisnika"),
          const SizedBox(height: 10),
          const Icon(
            Icons.person,
            color: Colors.amber,
            size: 50,
          ),
        ],
      ),
      contentPadding: const EdgeInsets.fromLTRB(30.0, 5, 30.0, 40),
      content: SizedBox(
        width: 600.0,
        child: FormBuilder(
          key: _formKey,
          initialValue: _initialValue,
          child: SingleChildScrollView(
            child: Column(
              children: [
                Row(
                  children: [
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'firstName',
                        decoration: const InputDecoration(labelText: 'Ime'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(30,
                              errorText: "Neispravan unos"),
                        ]),
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'lastName',
                        decoration: const InputDecoration(labelText: 'Prezime'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(30,
                              errorText: "Neispravan unos")
                        ]),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 10),
                Row(
                  children: [
                  Expanded(
                      child: FormBuilderTextField(
                        name: 'birthDate',
                        decoration: const InputDecoration(labelText: 'Datum rođenja'),
                        inputFormatters: [
                          MaskedInputFormatter('00-00-0000'),
                        ],
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(30, errorText: "Neispravan unos"),
                        ]),
                        keyboardType: TextInputType.number,
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'residencePlace',
                        decoration: const InputDecoration(labelText: 'Mjesto prebivališta'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(30,
                              errorText: "Neispravan unos")
                        ]),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 10),
                Row(
                  children: [
                   Expanded(
                      child: FormBuilderTextField(
                        name: 'email',
                        decoration: const InputDecoration(labelText: 'Email'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(30,
                              errorText: "Nevažeća email adresa"),
                          FormBuilderValidators.email(
                              errorText: "Nevažeća email adresa"),
                        ]),
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'phoneNumber',
                        decoration:
                            const InputDecoration(labelText: 'Kontakt telefon'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(20,
                              errorText: "Neispravan unos"),
                          FormBuilderValidators.match(r'^[0-9+\-]+$',
                              errorText: "Neispravan format"),
                        ]),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 10),
                if (AuthStorageProvider.getAuthData()?["role"] == Roles.admin)
                Row(
                  children: [
                    Expanded(
                      child: DropdownButtonFormField<Roles?>(
                          decoration: const InputDecoration(
                            labelText: 'Izaberite tip korisnik',
                          ),
                          value: selectedUserType,
                          items: widget.userTypeDropdown?.expand((item) {
                            return item.entries.map((entry) {
                              return DropdownMenuItem<Roles?>(
                                value: entry.key,
                                child: Text(entry.value),
                              );
                            });
                          }).toList(),
                          onChanged: (Roles? value) {
                            setState(() {
                              selectedUserType = value;
                            });
                          },
                          validator: (value) {
                          if (value == null) {
                            return 'Polje je obavezno';
                          }
                          return null;
                        },
                          ),
                    ),
                    const SizedBox(width: 60),
                    if (selectedUserType == Roles.coordinator ||
                        selectedUserType == Roles.touristGuide)
                      Expanded(
                        child: DropdownButtonFormField<dynamic>(
                          decoration: const InputDecoration(
                            labelText: 'Izaberite agenciju',
                          ),
                          value: selectedAgency,
                          items: widget.agenciesDropdown?.map((DropdownModel item) {
                            return DropdownMenuItem<dynamic>(
                              value: item.id,
                              child: Text(item.name ?? ""),
                            );
                          }).toList(),
                          onChanged: (value) {
                            setState(() {
                              selectedAgency = value;
                            });
                          },
                        ),
                      ),
                    if (selectedUserType != Roles.touristGuide &&
                        selectedUserType != Roles.coordinator)
                      const Spacer()
                  ],
                ),
                const SizedBox(height: 30),
                if (widget.user?.id == null)
                Row(children: [
                  Text("Napomena: ",
                      style: TextStyle(
                          fontSize: 10, fontWeight: FontWeight.bold)),
                  Text("Nakon kreiranja profila automatski će biti poslan mail korisniku sa njegovim pristupnim podacima.",
                      style: TextStyle(
                          fontSize: 10)),
                ]),
                const SizedBox(height: 30),
                Row(
                  mainAxisAlignment: MainAxisAlignment.end,
                  children: [
                    ElevatedButton(
                      onPressed: () {
                        Navigator.of(context).pop();
                      },
                      style: const ButtonStyle(
                        backgroundColor:
                            MaterialStatePropertyAll<Color>(Colors.white70),
                      ),
                      child: const Text('Odustani'),
                    ),
                    const SizedBox(width: 8),
                    ElevatedButton(
                      onPressed: () async {
                        if (_formKey.currentState?.saveAndValidate() ?? false) {
                          await submit();

                          Navigator.of(context).pop();
                        }
                      },
                      child: const Text(
                        'Sačuvaj',
                        style: TextStyle(color: Colors.white),
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Future<void> submit() async {
    try {
      var request = new Map.from(_formKey.currentState!.value);

      request["role"] = selectedUserType!.index + 1;
      request["agencyId"] = selectedAgency;
      if (widget.user?.id == null) {
        await _userProvider?.insert(request);
      } else {
        await _userProvider?.update(widget.user?.id, request);
      }
      widget.onCompleted();

      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Korisnik uspješno dodan!",
          backgroundColor: Colors.green);
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    }
  }
}
