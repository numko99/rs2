import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/arrangement_type.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/models/accomodation_type_form_controllers.dart';
import 'package:ui/models/arrangement_price.dart';
import 'package:ui/models/accomodation.dart';
import 'package:ui/models/address.dart';
import 'package:ui/models/arrangment.dart';
import 'package:ui/models/destination.dart';
import 'package:ui/models/destinatios_form_controllers.dart';
import 'package:ui/models/image_model.dart';
import 'package:ui/services/arrangment_provider.dart';
import 'package:ui/services/auth_storage_provider.dart';
import 'package:ui/widgets/layout.dart';
import 'package:ui/widgets/arrangement_add_edit/basic_data_form.dart';
import 'package:ui/widgets/arrangement_add_edit/description_images_form.dart';
import 'package:ui/widgets/arrangement_add_edit/destinations_form.dart';

class ArrangementAddEditPage extends StatefulWidget {
  const ArrangementAddEditPage(
      {super.key, this.agencyId, this.arrangementId});

  final String? agencyId;
  final String? arrangementId;

  @override
  State<ArrangementAddEditPage> createState() => _ArrangementAddEditPageState();
}

class _ArrangementAddEditPageState extends State<ArrangementAddEditPage> {
  final List<GlobalKey<FormBuilderState>> formKeys = [
    GlobalKey<FormBuilderState>(),
    GlobalKey<FormBuilderState>(),
    GlobalKey<FormBuilderState>(),
  ];

  ArrangmentProvider? _arrangmentProvider;
  List<ImageModel>? images = [];
  ImageModel? mainImage;
  bool displayMainImageValidationMsg = false;
  bool displayImagesValidationMsg = false;

  bool displayLoader = false;

  var accomodationTypeFormControllers = AccomodationTypeFormControllers();
  var destinationsFormControllers = DestinatiosFormControllers();

  int currentStep = 0;
  ArrangementType initialArrangemetType = ArrangementType.multiDayTrip;
  ArrangementType arrangementType = ArrangementType.multiDayTrip;
  String? agencyId;
  Arrangement? arrangement;
  Map<String, dynamic> _initialValue1 = {};
  Map<String, dynamic> _initialValue2 = {};
  

  @override
  void initState() {
    super.initState();
    _arrangmentProvider = context.read<ArrangmentProvider>();
    loadData();
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
      displayBackNavigationArrow: true,
      name: "Aranžmani",
      icon: Icons.beach_access_outlined,
      body: Card(
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(children: [
            const Text('Dodaj aranžman'),
            const SizedBox(height: 10),
            const Icon(
              Icons.beach_access_outlined,
              color: Colors.amber,
              size: 50,
            ),
            if (displayLoader)
              const CircularProgressIndicator()
            else
              Padding(
                  padding: const EdgeInsets.fromLTRB(0, 15, 30.0, 40),
                  child: Stepper(
                    currentStep: currentStep,
                    onStepContinue: currentStep < 2 ? increaseStep : submitData,
                    onStepCancel: currentStep > 0
                        ? () => setState(() => currentStep--)
                        : null,
                    controlsBuilder:
                        (BuildContext context, ControlsDetails details) {
                      return Row(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: <Widget>[
                          ElevatedButton(
                            onPressed: details.onStepContinue,
                            style: ElevatedButton.styleFrom(
                                backgroundColor: Colors.amber),
                            child: Text(currentStep < 2 ? 'Dalje' : 'Završi',
                                style: const TextStyle(
                                  fontSize: 15,
                                  color: Colors.white,
                                )),
                          ),
                          SizedBox(width: 10),
                          ElevatedButton(
                            onPressed: details.onStepCancel,
                            child: const Text('Nazad',
                                style: TextStyle(
                                    fontSize: 15, color: Colors.white)),
                            style: ElevatedButton.styleFrom(
                              backgroundColor: Colors.grey,
                            ),
                          ),
                        ],
                      );
                    },
                    steps: [
                      Step(
                        title: const Text('Osnovni podaci'),
                        content: FormBuilder(
                            key: formKeys[0],
                            initialValue: _initialValue1,
                            child: BasicDataFormPage(
                                agencyId: widget.agencyId ?? arrangement?.agency.id,
                                controllers: accomodationTypeFormControllers,
                                setArrangementType: setArrangementType,
                                setAgencyId: setAgency,
                                arrangementStatus: arrangement?.arrangementStatusId,
                                selectedStartDate: arrangement?.startDate ?? DateTime.now(),
                                initialArrangementType: initialArrangemetType)),
                        isActive: currentStep == 0,
                      ),
                      Step(
                        title: const Text('Slike i Opis'),
                        content: FormBuilder(
                            key: formKeys[1],
                            initialValue: _initialValue2,
                            child: DescriptionImageFormPage(
                              images: images,
                              mainImage: mainImage,
                              displayImagesValidationMsg:
                                  displayImagesValidationMsg,
                              displayMainImageValidationMsg:
                                  displayMainImageValidationMsg,
                              onUpdateImages: updateImages,
                              onUpdateMainImage: updateMainImage,
                            )),
                        isActive: currentStep == 1,
                      ),
                      Step(
                        title: const Text('Destinacije i smještaji'),
                        content: FormBuilder(
                            key: formKeys[2],
                            child: DestinationFormPage(
                                controllers: destinationsFormControllers)),
                        isActive: currentStep == 2,
                      ),
                    ],
                  )),
          ]),
        ),
      ),
    );
  }

  Future<void> submitData() async {
    try {
      if (formKeys[currentStep].currentState?.saveAndValidate() ?? false) {
        var basicDataFormData = Map.from(formKeys[0].currentState!.value);

        var processedBasicData = processBasicData(basicDataFormData);
        var descriptionAndImagesFormData =
            Map.from(formKeys[1].currentState!.value);
        var procesedDescriptionAndImagesFormData =
            await processDescriptionAndImagesData(descriptionAndImagesFormData);

        var procesedDestinatiosFormData = processDestinationsData();

        var finalFormData = {
          ...processedBasicData,
          ...procesedDescriptionAndImagesFormData
        };
        finalFormData["destinations"] = procesedDestinatiosFormData;

        if (widget.arrangementId == null) {
          await _arrangmentProvider!.insert(finalFormData);
        } else {
          await _arrangmentProvider!
              .update(widget.arrangementId, finalFormData);
        }

        ScaffoldMessengerHelper.showCustomSnackBar(
            context: context,
            message:widget.arrangementId == null ? "Aranžman uspješno dodan!" : "Aranžman uspješno uređen!",
            backgroundColor: Colors.green);

           Navigator.pushNamed(context, '/arrangements');

      }
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    } finally {
      setState(() => displayLoader = false);
    }
  }

  Map<dynamic, dynamic> processBasicData(Map<dynamic, dynamic> formData) {
    List<ArrangementPrice> prices = [];
    if (arrangementType == ArrangementType.multiDayTrip) {
      for (int i = 0;
          i < accomodationTypeFormControllers.accomodationTypePrices.length;
          i++) {
        var price = double.tryParse(accomodationTypeFormControllers
                .accomodationTypePrices[i].text) ??
            0;
        prices.add(ArrangementPrice(
            id: accomodationTypeFormControllers.accomodationTypeIds[i],
            accommodationType:
                accomodationTypeFormControllers.accomodationTypes[i].text,
            price: price));
      }
    }
 DateTime startDate = formData["startDate"];
    DateTime? endDate = formData["endDate"];

    startDate = startDate.add(const Duration(hours: 2));
    if (endDate != null) {
      endDate = endDate.add(const Duration(hours: 2));
    }

    formData["startDate"] = startDate.toIso8601String();
    if (endDate != null){
      formData["endDate"] = endDate.toIso8601String();
    }
    formData["prices"] = prices;
    formData["price"] = arrangementType == ArrangementType.multiDayTrip ? null : formData["price"];
    formData["agencyId"] = agencyId ?? widget.agencyId ?? AuthStorageProvider.getAuthData()?["agencyId"];


    return formData;
  }

  Future<Map<dynamic, dynamic>> processDescriptionAndImagesData(
      Map<dynamic, dynamic> formData) async {
    formData["mainImage"] = mainImage!.toJson();
    formData["images"] = images!.map((image) => image.toJson()).toList();
    return formData;
  }

  List<Destination> processDestinationsData() {
    List<Destination> destinationList = [];
    DateFormat format = DateFormat("dd-MM-yyyy HH:mm");

    for (int i = 0;
        i < destinationsFormControllers.destinationControllers.length;
        i++) {
      var arrivalDate = format.parse(
          destinationsFormControllers.arrivalDateTimeControllers[i].text);
      var departureDate = format.parse(
          destinationsFormControllers.departureDateTimeControllers[i].text);

      bool isOneDayTrip =
          destinationsFormControllers.hotelNameControllers[i].text.isEmpty;

      var destination = Destination(
          id: destinationsFormControllers.idControllers[i].text.isEmpty
              ? null
              : destinationsFormControllers.idControllers[i].text,
          city: destinationsFormControllers.destinationControllers[i].text,
          country: destinationsFormControllers.countryControllers[i].text,
          arrivalDate: arrivalDate,
          departureDate: departureDate,
          isOneDayTrip: isOneDayTrip,
          accommodation: !isOneDayTrip
              ? Accomodation(
                  hotelName:
                      destinationsFormControllers.hotelNameControllers[i].text,
                  checkInDate: arrivalDate,
                  checkOutDate: departureDate,
                  hotelAddress: Address(
                    street:
                        destinationsFormControllers.streetControllers[i].text,
                    houseNumber: destinationsFormControllers
                        .houseNumberControllers[i].text,
                    cityId: destinationsFormControllers.destinationControllers[i].text,
                    postalCode: destinationsFormControllers
                        .postalCodeControllers[i].text,
                    country:
                        destinationsFormControllers.countryControllers[i].text,
                  ),
                )
              : null);

      destinationList.add(destination);
    }

    return destinationList;
  }

  void updateImages(List<ImageModel> updatedImages) {
    setState(() {
      images = updatedImages;
    });
  }

  void updateMainImage(ImageModel? updatedMainImage) {
    setState(() {
      mainImage = updatedMainImage;
    });
  }

  void setArrangementType(ArrangementType? value){
    setState(() {
      arrangementType = value!;
    });
  }

  void setAgency(String? agency) {
    setState(() {
      agencyId = agency!;
    });
  }

  Future<void> increaseStep() async {
    setState(() {
      displayMainImageValidationMsg = currentStep == 1 && (mainImage == null);
      displayImagesValidationMsg =
          currentStep == 1 && (images?.isEmpty ?? true);
    });

    var validFormData =
        formKeys[currentStep].currentState?.saveAndValidate() ?? false;
    if (validFormData &&
        !displayMainImageValidationMsg &&
        !displayImagesValidationMsg) {
      setState(() {
        currentStep = currentStep + 1;
      });
    }
  }

  Future<void> loadData() async {
    if (widget.arrangementId == null) {
      return;
    }

    try {
      setState(() {
        displayLoader = true;
      });
      arrangement = await _arrangmentProvider?.getById(widget.arrangementId);
      agencyId = arrangement!.agency.id;
      setInitialBasicData();
      setInitialDescriptionAndImagesData();
      setInitialDestinationsData();
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        displayLoader = false;
      });
    }
  }

  void setInitialDestinationsData() {
    var destinations = arrangement!.destinations;
    for (int i = 0; i < destinations.length; i++) {
      destinationsFormControllers.addDestination();
      destinationsFormControllers.idControllers[i].text = destinations[i].id!;
      destinationsFormControllers.destinationControllers[i].text =
          destinations[i].cityId.toString()!;
      destinationsFormControllers.countryControllers[i].text =
          destinations[i].countryId.toString()!;
      if (destinations[i].arrivalDate != null) {
        String formattedDate =
            DateFormat('dd-MM-yyyy HH:mm').format(destinations[i].arrivalDate!);
        destinationsFormControllers.arrivalDateTimeControllers[i].text =
            formattedDate;
      }

      if (destinations[i].departureDate != null) {
        String formattedDate = DateFormat('dd-MM-yyyy HH:mm')
            .format(destinations[i].departureDate!);
        destinationsFormControllers.departureDateTimeControllers[i].text =
            formattedDate;
      }
      destinationsFormControllers.isHotelIncluded[i] =
          destinations[i].accommodation != null;
      if (destinations[i].accommodation != null) {
        destinationsFormControllers.hotelNameControllers[i].text =
            destinations[i].accommodation!.hotelName!;

        if (destinations[i].accommodation?.checkInDate != null) {
          String formattedDate = DateFormat('dd-MM-yyyy HH:mm')
              .format(destinations[i].accommodation!.checkInDate!);
          destinationsFormControllers.arrivalDateTimeControllers[i].text =
              formattedDate;
        }
        if (destinations[i].accommodation?.checkOutDate != null) {
          String formattedDate = DateFormat('dd-MM-yyyy HH:mm')
              .format(destinations[i].accommodation!.checkOutDate!);
          destinationsFormControllers.departureDateTimeControllers[i].text =
              formattedDate;
        }
        destinationsFormControllers.streetControllers[i].text =
            destinations[i].accommodation!.hotelAddress!.street!;
        destinationsFormControllers.houseNumberControllers[i].text =
            destinations[i].accommodation!.hotelAddress!.houseNumber!;
        destinationsFormControllers.postalCodeControllers[i].text =
            destinations[i].accommodation!.hotelAddress!.postalCode!;
      }
    }
  }

  void setInitialDescriptionAndImagesData() {
    _initialValue2 = {
      "description": arrangement?.description,
      "shortDescription": arrangement?.shortDescription,
    };

    mainImage = arrangement!.images
        .where((element) => element.isMainImage == true)
        .firstOrNull;
    images = arrangement!.images
        .where((element) => element.isMainImage == false)
        .toList();
  }

  void setInitialBasicData() {
    var pricess = arrangement!.prices;
    initialArrangemetType = pricess.length == 1 && pricess[0].accommodationType == null ? ArrangementType.oneDayTrip : ArrangementType.multiDayTrip;
    setArrangementType(initialArrangemetType);
    _initialValue1 = {
      "name": arrangement?.name,
      "startDate": arrangement?.startDate,
      "endDate": arrangement?.endDate,
      "price": initialArrangemetType == 2 ? pricess[0].price.toString() : null
    };

    if (initialArrangemetType == ArrangementType.multiDayTrip) {
      for (int i = 0; i < pricess.length; i++) {
          accomodationTypeFormControllers.addArrangementPrices();
        accomodationTypeFormControllers.accomodationTypePrices[i].text =
            pricess[i].price.toString();
        accomodationTypeFormControllers.accomodationTypes[i].text =
            pricess[i].accommodationType!;
        accomodationTypeFormControllers.accomodationTypeIds[i] = pricess[i].id;
      }
    }
  }
}
